using System;
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

        public WindowTilingType windowTilingType = WindowTilingType.Bugn;
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
            this.currentWindowInfoIndex = 0;
        }

        public int WindowCount()
        {
            return windowInfos.Count();
        }

        // ウィンドウリストの先頭に追加する
        public void Push(WindowInfoWithHandle windowInfo)
        {
            this.windowInfos.Insert(0, windowInfo);

            // 追加したらこれをカレントウィンドウにする
            this.currentWindowInfoIndex = 0;
        }


        // ウィンドウリストに追加する
        public void Add(WindowInfoWithHandle windowInfo)
        {
            this.windowInfos.Add(windowInfo);

            // 追加したらこれをカレントウィンドウにする
            this.currentWindowInfoIndex = this.windowInfos.Count - 1;
        }

        // ウィンドウリストから削除する
        public void Remove(WindowInfoWithHandle windowInfo)
        {
            if (this.windowInfos.Contains(windowInfo))
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
            if ( this.windowInfos.Count <= this.currentWindowInfoIndex)
            {
                return null;
            }
            var windowInfoWithHandle = this.windowInfos.ElementAt(this.currentWindowInfoIndex);
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
            this.currentWindowInfoIndex = windowIndex;
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

                    this.MoveWindow( windowHandle,asIsWindowRect);
                    continue;
                }

                if (! toBeWindowRect.Equals(currentWindowRect))
                {
                    this.MoveWindow( windowHandle,toBeWindowRect);
                }
            }
        }

        public void MoveWindow( IntPtr hWnd, WindowRect windowRect)
        {
            // 最大化、最小化Windowの場合は元のウィンドウにする
            if ( IsZoomed(hWnd) || IsIconic(hWnd))
            {
                ShowWindow(hWnd, /* SW_RESTORE = */ 9);
            }

            this.MoveWindow(hWnd, windowRect);
        }

        /**
         * <summary>
         * フォーカスするウィンドウをひとつ前に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusPrevious()
        {
            // リストの先頭なら、現在のウィンドウを戻す
            if (  this.currentWindowInfoIndex == 0) 
            {
                return this.GetCurrentWindow();
            }

            this.currentWindowInfoIndex -=  1;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウをひとつ後に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusNext()
        {
            // リストの最後なら、現在のウィンドウを戻す
            if ( this.currentWindowInfoIndex == this.windowInfos.Count() - 1)
            {
                return this.GetCurrentWindow();
            }

            this.currentWindowInfoIndex += 1;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを先頭に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusTop()
        {
            this.currentWindowInfoIndex = 0;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを最後に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusBottom()
        {
            this.currentWindowInfoIndex = this.windowInfos.Count() - 1;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置をひとつ前に移動する
         * </summary>
         */
        public WindowInfoWithHandle SetWindowPrevious()
        {
            var currentIndex = this.currentWindowInfoIndex;
            var previousIndex = 0;
            if (currentIndex > 0)
            {
                previousIndex = currentIndex - 1;
            }
            if (currentIndex != previousIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, previousIndex);
                this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                    this.windowInfos.ElementAt(previousIndex));
                this.currentWindowInfoIndex = previousIndex;
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
            var currentIndex = this.currentWindowInfoIndex;
            var nextIndex = this.windowInfos.Count() - 1;
            if (currentIndex < this.windowInfos.Count() - 1 )
            {
                nextIndex = currentIndex + 1;
            }
            if (currentIndex != nextIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, nextIndex);
                this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                    this.windowInfos.ElementAt(nextIndex));
                this.currentWindowInfoIndex = nextIndex;
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
            var currentIndex = this.currentWindowInfoIndex;
            var topIndex = 0;
            if (currentIndex != topIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, topIndex);
                this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                    this.windowInfos.ElementAt(topIndex));
                this.currentWindowInfoIndex = topIndex;
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
            var currentIndex = this.currentWindowInfoIndex;
            var bottomIndex = this.windowInfos.Count() - 1;
            if ( currentIndex != bottomIndex)
            {
                this.Exchange(this.windowInfos, currentIndex, bottomIndex);
                this.ChangeWindowPosition(this.windowInfos.ElementAt(currentIndex),
                    this.windowInfos.ElementAt(bottomIndex));
                this.currentWindowInfoIndex = bottomIndex;
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

            this.MoveWindow(srcWindowInfoWithHandle.windowHandle, destWindowRect);

            this.MoveWindow(destWindowInfoWithHandle.windowHandle, srcWindowRect);
        }

    }
}
