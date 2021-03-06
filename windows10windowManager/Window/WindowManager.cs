﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using windows10windowManager.Window;
using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    public class WindowManager : IEquatable<WindowManager>
    {

        #region Field
        public IntPtr monitorHandle { get; private set; }

        // このモニター内で管理しているウィンドウ情報
        // このリストの並び順でウィンドウを整列させる
        protected List<WindowInfoWithHandle> windowInfos { get; set; }

        protected int currentWindowInfoIndex { get; set; }

        public WindowTilingType windowTilingType;

        private readonly object windowInfosLock = new object();
        #endregion

        #region WinApi
        [DllImport("user32")]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y,
            int nWidth, int nHeight, int bRepaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        public WindowManager(IntPtr monitorHandle)
        {
            this.monitorHandle = monitorHandle;
            this.windowInfos = new List<WindowInfoWithHandle>();
            this.SetCurrentWindowIndex( 0);
        }

        public void SaveWindowTilingType(int currentWindowManagerIndex, WindowTilingType windowTilingType)
        {
            this.windowTilingType = windowTilingType;
            SettingManager.SaveInt($"Window_WindowManager{currentWindowManagerIndex}_WindowTilingType", 
                (int)windowTilingType);
        }


        public int WindowCount()
        {
            return windowInfos.Count();
        }

        /**
         * <summary>
         * 新規ウィンドウを WindowTilingType に基づいて先頭に追加する
         * </summary>
         */
        public void PushNew(WindowInfoWithHandle windowInfo)
        {
            lock (this.windowInfosLock)
            {
                int pushedIndex = WindowTiler.PushNewWindowInfo(this.windowTilingType, this.windowInfos, windowInfo);

                this.currentWindowInfoIndex = pushedIndex;
            }
        }

        /**
         * <summary>
         * ウィンドウリストの先頭に追加する
         * </summary>
         */
        public void Push(WindowInfoWithHandle windowInfo)
        {
            lock (this.windowInfosLock)
            {
                this.windowInfos.Insert(0, windowInfo);

                // 追加したらこれをカレントウィンドウにする
                this.currentWindowInfoIndex = 0;
            }
        }


        /**
         * <summary>
         * ウィンドウリストに追加する
         * </summary>
         */
        public void Add(WindowInfoWithHandle windowInfo)
        {
            lock (this.windowInfosLock)
            {
                this.windowInfos.Add(windowInfo);

                // 追加したらこれをカレントウィンドウにする
                this.currentWindowInfoIndex = this.windowInfos.Count - 1;
            }
        }

        // ウィンドウリストから削除する
        public void Remove(WindowInfoWithHandle windowInfo)
        {
            if (this.windowInfos.Contains(windowInfo))
            {
                lock (this.windowInfosLock)
                {
                    this.windowInfos.Remove(windowInfo);

                    // 削除したら一つ上をカレントウィンドウにする
                    // 0 なら 0 のまま
                    if (this.currentWindowInfoIndex > 0)
                    {
                        this.currentWindowInfoIndex -= 1;
                    }
                }
            }
        }

        public bool Equals(WindowManager other)
        {
            return this.monitorHandle == other.monitorHandle;
        }

        // モニター内のウィンドウリストを管理する
        public List<WindowInfoWithHandle> GetWindows()
        {
            return windowInfos;
        }

        /**
         * <summary>
         * カレントウィンドウを取得する
         * </summary> 
         */
        public WindowInfoWithHandle GetCurrentWindow()
        {
            if ( this.windowInfos.Count == 0)
            {
                return null;
            }
            if ( this.windowInfos.Count <= this.GetCurrentWindowIndex())
            {
                return null;
            }
            var windowInfoWithHandle = this.windowInfos.ElementAt(this.GetCurrentWindowIndex());
            Logger.DebugWindowInfo("WindowManager.GetCurrentWindow", windowInfoWithHandle);
            return windowInfoWithHandle;
        }

        /**
         * <summary>
         * カレントウィンドウの WindowInfos 内のインデックスを取得する
         * </summary>
         */
        public int GetCurrentWindowIndex()
        {
            return this.currentWindowInfoIndex;
        }


        public void SetCurrentWindowIndex(int windowIndex)
        {
            Logger.WriteLine($"WindowManager.SetCurrentWindowIndex = {windowIndex}");
            if ( windowIndex < 0 || windowIndex >= this.windowInfos.Count())
            {
                windowIndex = this.GetCurrentWindowIndex();
                Logger.WriteLine($"WindowManager.SetCurrentWindowIndex = {windowIndex}(Changed)");
            }
            if (windowIndex < 0 || windowIndex >= this.windowInfos.Count())
            {
                windowIndex = 0;
                Logger.WriteLine($"WindowManager.SetCurrentWindowIndex = {windowIndex}(Changed)");
            }
            lock ( this.windowInfosLock)
            {
                this.currentWindowInfoIndex = windowIndex;
            }
        }

        /**
         * <summary>
         * WindowInfowWithHandleをカレントウィンドウにする
         * </summary>
         */
        public void SetCurrentWindowIndexByWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            var index = this.windowInfos.FindIndex(
                (WindowInfoWithHandle needleWindowInfo) => { return windowInfoWithHandle.Equals(needleWindowInfo); });
            this.SetCurrentWindowIndex(index);
        }

        /**
         * <summary>
         * すでに表示されていないウィンドウをWindowInfosから削除する
         * </summary>
         */
        public void RemoveInvisibleWindows()
        {
            for (int i = 0; i < this.windowInfos.Count; i++)
            {
                var windowInfoWithHandle = this.windowInfos[i];
                if (! windowInfoWithHandle.IsValid())
                {
                    this.Remove(windowInfoWithHandle);
                }
            }

        }

        /**
         * <summary>
         * windowTiler で取得できる位置情報に基づいてウィンドウの位置を移動する
         * </summary>
         */
        public void ArrangeWindows(WindowTiler windowTiler)
        {
            for (int i = 0; i < this.windowInfos.Count; i++)
            {
                var windowInfoWithHandle = this.windowInfos[i];
                var windowHandle = windowInfoWithHandle.windowHandle;
                var currentWindowRect = windowInfoWithHandle.GetCurrentWindowRect();
                var toBeWindowRect = windowTiler.GetWindowRectOf(i);

                // toBeWindowRect == null の場合はもとの位置に戻す
                if ( toBeWindowRect == null)
                {
                    var asIsWindowRect = windowInfoWithHandle.GetOriginalWindowRect();

                    this.MoveWindow(windowInfoWithHandle, asIsWindowRect);
                    continue;
                }

                if (! toBeWindowRect.Equals(currentWindowRect))
                {
                    this.MoveWindow(windowInfoWithHandle, toBeWindowRect);
                }
            }
        }

        /**
         * <summary>
         * 指定されたWindowInfoWithHandleをWindowRectの位置に移動する
         * ただしWindowInfoWithHandle.positionXXAdjustmentで位置を補正して隙間ができないようにする
         * </summary>
         */
        public void MoveWindow( WindowInfoWithHandle windowInfoWithHandle, WindowRect windowRect)
        {
            var hWnd = windowInfoWithHandle.windowHandle;
            var windowRectString = windowRect.ToString();
            Logger.WriteLine($"WindowManager.MoveWindow : hWnd={hWnd} To {windowRectString}");


            // 最大化、最小化Windowの場合は元のウィンドウにする
            // if ( IsZoomed(hWnd) || IsIconic(hWnd))
            if (IsZoomed(hWnd))
            {
                ShowWindow(hWnd, /* SW_RESTORE = */ 9);
            }
            

            MoveWindow(hWnd, 
                windowRect.GetX() + windowInfoWithHandle.positionLeftAdjustment,
                windowRect.GetY() + windowInfoWithHandle.positionTopAdjustment,
                windowRect.GetWidth() + windowInfoWithHandle.positionWidthAdjustment,
                windowRect.GetHeight() + windowInfoWithHandle.positionHeightAdjustment,
                /* bRepaint = */1);
        }

        /**
         * <summary>
         * フォーカスするウィンドウをひとつ前に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusPrevious()
        {
            // リストの先頭なら、現在のウィンドウを戻す
            if (  this.GetCurrentWindowIndex() == 0) 
            {
                return this.GetCurrentWindow();
            }

            this.SetCurrentWindowIndex(this.GetCurrentWindowIndex()-1);
            return this.GetCurrentWindow();
        }

        /**m
         * <summary>
         * フォーカスするウィンドウをひとつ後に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusNext()
        {
            // リストの最後なら、現在のウィンドウを戻す
            if ( this.GetCurrentWindowIndex() == this.windowInfos.Count() - 1)
            {
                return this.GetCurrentWindow();
            }

            this.SetCurrentWindowIndex(this.GetCurrentWindowIndex() + 1);
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを先頭に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusTop()
        {
            this.SetCurrentWindowIndex(0);
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを最後に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusBottom()
        {
            this.SetCurrentWindowIndex(this.windowInfos.Count() - 1);
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置をひとつ前に移動する
         * </summary>
         */
        public WindowInfoWithHandle SetWindowPrevious()
        {
            if (this.windowInfos.Count() < 2)
            {
                return null;
            }
            var currentIndex = this.GetCurrentWindowIndex();
            var previousIndex = 0;
            if (currentIndex > 0)
            {
                previousIndex = currentIndex - 1;
            }
            if (currentIndex != previousIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, previousIndex);
                //this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                //  this.windowInfos.ElementAt(previousIndex));
                this.SetCurrentWindowIndex(previousIndex);
                return this.windowInfos.ElementAt(previousIndex);
            }
            return this.windowInfos.ElementAt(currentIndex);
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置をひとつ後に移動する
         * </summary>
         */
        public WindowInfoWithHandle SetWindowNext()
        {
            if ( this.windowInfos.Count() < 2)
            {
                return null;
            }
            var currentIndex = this.GetCurrentWindowIndex();
            var nextIndex = this.windowInfos.Count() - 1;
            if (currentIndex < this.windowInfos.Count() - 1 )
            {
                nextIndex = currentIndex + 1;
            }
            if (currentIndex != nextIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, nextIndex);
                //this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                //  this.windowInfos.ElementAt(nextIndex));
                this.SetCurrentWindowIndex(nextIndex);
                return this.windowInfos.ElementAt(nextIndex);
            }
            return this.windowInfos.ElementAt(currentIndex);
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置を先頭に移動する
         * </summary>
         */
        public WindowInfoWithHandle SetWindowTop()
        {
            var currentIndex = this.GetCurrentWindowIndex();
            var topIndex = 0;
            if (currentIndex != topIndex)
            {
                //this.Exchange(this.windowInfos, currentIndex, topIndex);
                var currentWindowInfo = this.windowInfos.ElementAt(currentIndex);
                this.Remove(currentWindowInfo);
                this.Push(currentWindowInfo);
                //this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                //    this.windowInfos.ElementAt(topIndex));
                this.SetCurrentWindowIndex(topIndex);
                return this.windowInfos.ElementAt(topIndex);
            }
            return this.windowInfos.ElementAt(currentIndex);
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置を最後に移動する
         * </summary>
         */
        public WindowInfoWithHandle SetWindowBottom()
        {
            var currentIndex = this.GetCurrentWindowIndex();
            var bottomIndex = this.windowInfos.Count() - 1;
            if ( currentIndex != bottomIndex)
            {
                //this.Exchange(this.windowInfos, currentIndex, bottomIndex);
                var currentWindowInfo = this.windowInfos.ElementAt(currentIndex);
                this.Remove(currentWindowInfo);
                this.Add(currentWindowInfo);
                //this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                //  this.windowInfos.ElementAt(bottomIndex));
                this.SetCurrentWindowIndex( bottomIndex);
                return this.windowInfos.ElementAt(bottomIndex);
            }
            return this.windowInfos.ElementAt(currentIndex);
        }

        /**
         * <summary>
         * リストの位置を入替する
         * </summary>
         */
        protected void Exchange<T>(List<T> list, int oldIndex, int newIndex)
        {
            T aux = list[newIndex];
            list[newIndex] = list[oldIndex];
            list[oldIndex] = aux;
        }

        /**
         * <summary>
         * src と dest のウィンドウの位置を入れ替える
         * </summary>
         */
        public void ChangeWindowPosition(WindowInfoWithHandle srcWindowInfoWithHandle, 
            WindowInfoWithHandle destWindowInfoWithHandle)
        {
            if ( this.windowTilingType == WindowTilingType.None )
            {
                return;
            }

            WindowRect srcWindowRect = srcWindowInfoWithHandle.GetCurrentWindowRect();
            WindowRect destWindowRect = destWindowInfoWithHandle.GetCurrentWindowRect();

            this.MoveWindow(srcWindowInfoWithHandle, destWindowRect);

            this.MoveWindow(destWindowInfoWithHandle, srcWindowRect);
        }

    }
}
