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
        public IntPtr MonitorHandle { get; private set; }

        // このモニター内で管理しているウィンドウ情報
        // このリストの並び順でウィンドウを整列させる
        protected List<WindowInfoWithHandle> WindowInfos { get; set; }

        protected int CurrentWindowInfoIndex { get; set; }

        /**
         * <summary>
         * 1: m: 全画面
         * 2: t: タイル(bug.nのような)
         * 3: w: 四分割
         * 4: f: MDI的な感じ
         * </summary>
         */
        protected int WindowArrangementMode { get; set; } = 0;

        [DllImport("user32")]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y,
            int nWidth, int nHeight, int bRepaint);

        public WindowManager(IntPtr monitorHandle)
        {
            this.MonitorHandle = monitorHandle;
            this.WindowInfos = new List<WindowInfoWithHandle>();
            this.CurrentWindowInfoIndex = 0;
        }

        public int WindowCount()
        {
            return WindowInfos.Count();
        }

        // ウィンドウリストに追加する
        public void Add(WindowInfoWithHandle windowInfo)
        {
            this.WindowInfos.Add(windowInfo);

            // 追加したらこれをカレントウィンドウにする
            this.CurrentWindowInfoIndex = this.WindowInfos.Count - 1;
        }

        // ウィンドウリストから削除する
        public void Remove(WindowInfoWithHandle windowInfo)
        {
            if (this.WindowInfos.Contains(windowInfo))
            {
                this.WindowInfos.Remove(windowInfo);

                // 削除したら一つ上をカレントウィンドウにする
                // 0 なら 0 のまま
                if (this.CurrentWindowInfoIndex > 0)
                {
                    this.CurrentWindowInfoIndex -= 1;
                }
            }
        }

        public bool Equals(WindowManager other)
        {
            return this.MonitorHandle == other.MonitorHandle;
        }

        // モニター内のウィンドウリストを管理する
        public List<WindowInfoWithHandle> GetWindows()
        {
            return WindowInfos;
        }

        /**
         * <summary>
         * カレントウィンドウを取得する
         * </summary> 
         */
        public WindowInfoWithHandle GetCurrentWindow()
        {
            if ( this.WindowInfos.Count <= this.CurrentWindowInfoIndex)
            {
                return null;
            }
            var windowInfoWithHandle = WindowInfos.ElementAt(this.CurrentWindowInfoIndex);
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
            return this.CurrentWindowInfoIndex;
        }

        public void SetCurrentWindowIndex(int windowIndex)
        {
            this.CurrentWindowInfoIndex = windowIndex;
        }

        public void SetWindowArrangementMode(int mode)
        {
            this.WindowArrangementMode = mode;
        }

        /**
         * <summary>
         * 現在の WindowArrangementMode に基づいてウィンドウを整列しなおす
         * </summary>
         */
        public void ArrangeWindows()
        {
            //TODO
        }

        /**
         * <summary>
         * フォーカスするウィンドウをひとつ前に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusPrevious()
        {
            // リストの先頭なら、現在のウィンドウを戻す
            if (  this.CurrentWindowInfoIndex == 0) 
            {
                return this.GetCurrentWindow();
            }

            this.CurrentWindowInfoIndex -=  1;
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
            if ( this.CurrentWindowInfoIndex == this.WindowInfos.Count() - 1)
            {
                return this.GetCurrentWindow();
            }

            this.CurrentWindowInfoIndex += 1;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを先頭に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusTop()
        {
            this.CurrentWindowInfoIndex = 0;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * フォーカスするウィンドウを最後に変更する
         * </summary>
         */
        public WindowInfoWithHandle MoveCurrentFocusBottom()
        {
            this.CurrentWindowInfoIndex = this.WindowInfos.Count() - 1;
            return this.GetCurrentWindow();
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置をひとつ前に移動する
         * </summary>
         */
        public void SetWindowPrevious()
        {
            var currentIndex = this.CurrentWindowInfoIndex;
            var previousIndex = 0;
            if (currentIndex > 0)
            {
                previousIndex = currentIndex - 1;
            }
            if (currentIndex != previousIndex)
            {
                this.Move(WindowInfos, currentIndex, previousIndex);
                this.ChangeWindowPosition(WindowInfos.ElementAt(currentIndex),
                    WindowInfos.ElementAt(previousIndex));
                this.CurrentWindowInfoIndex = previousIndex;
            }
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置をひとつ後に移動する
         * </summary>
         */
        public void SetWindowNext()
        {
            var currentIndex = this.CurrentWindowInfoIndex;
            var nextIndex = this.WindowInfos.Count() - 1;
            if (currentIndex < this.WindowInfos.Count() - 1 )
            {
                nextIndex = currentIndex + 1;
            }
            if (currentIndex != nextIndex)
            {
                this.Move(WindowInfos, currentIndex, nextIndex);
                this.ChangeWindowPosition(WindowInfos.ElementAt(currentIndex),
                    WindowInfos.ElementAt(nextIndex));
                this.CurrentWindowInfoIndex = nextIndex;
            }
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置を先頭に移動する
         * </summary>
         */
        public void SetWindowTop()
        {
            var currentIndex = this.CurrentWindowInfoIndex;
            var topIndex = 0;
            if (currentIndex != topIndex)
            {
                this.Move(WindowInfos, currentIndex, topIndex);
                this.ChangeWindowPosition(WindowInfos.ElementAt(currentIndex),
                    WindowInfos.ElementAt(topIndex));
                this.CurrentWindowInfoIndex = topIndex;
            }
        }

        /**
         * <summary>
         * アクティヴウィンドウの位置を最後に移動する
         * </summary>
         */
        public void SetWindowBottom()
        {
            var currentIndex = this.CurrentWindowInfoIndex;
            var bottomIndex = WindowInfos.Count() - 1;
            if ( currentIndex != bottomIndex)
            {
                this.Move(WindowInfos, currentIndex, bottomIndex);
                this.ChangeWindowPosition(WindowInfos.ElementAt(currentIndex),
                    WindowInfos.ElementAt(bottomIndex));
                this.CurrentWindowInfoIndex = bottomIndex;
            }
        }

        protected void Move<T>(List<T> list, int oldIndex, int newIndex)
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
            if ( this.WindowArrangementMode == 0 )
            {
                return;
            }

            RECT srcPosotion = srcWindowInfoWithHandle.Position;

            MoveWindow(srcWindowInfoWithHandle.WindowHandle,
                destWindowInfoWithHandle.GetPositionX(),
                destWindowInfoWithHandle.GetPositionY(),
                destWindowInfoWithHandle.GetPositionWidth(),
                destWindowInfoWithHandle.GetPositionHeight(),
                /* bRepaint = */ 1);

            MoveWindow(destWindowInfoWithHandle.WindowHandle,
                srcWindowInfoWithHandle.CalcPositionX(srcPosotion),
                srcWindowInfoWithHandle.CalcPositionY(srcPosotion),
                srcWindowInfoWithHandle.CalcPositionWidth(srcPosotion),
                srcWindowInfoWithHandle.CalcPositionHeight(srcPosotion),
                /* bRepaint = */ 1);

        }

    }
}
