using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using windows10windowManager.Window;

namespace windows10windowManager.Window
{
    public class WindowManager : IEquatable<WindowManager>
    {
        public IntPtr MonitorHandle { get; private set; }

        // このモニター内で管理しているウィンドウ情報
        // このリストの並び順でウィンドウを整列させる
        protected List<WindowInfoWithHandle> WindowInfos { get; set; }

        protected int CurrentWindowInfoIndex { get; set; }

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

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
            WindowInfos.Add(windowInfo);
        }

        // ウィンドウリストから削除する
        public void Remove(WindowInfoWithHandle windowInfo)
        {
            if (WindowInfos.Contains(windowInfo))
            {
                WindowInfos.Remove(windowInfo);
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

        // カレントウィンドウを取得する
        public WindowInfoWithHandle GetCurrentWindow()
        {
            return WindowInfos.ElementAt(CurrentWindowInfoIndex);
        }

        // カレントウィンドウの WindowInfos 内のインデックスを取得する
        public int GetCurrentWindowIndex()
        {
            return CurrentWindowInfoIndex;
        }

        public void SetCurrentWindowIndex(int windowIndex)
        {
            this.CurrentWindowInfoIndex = windowIndex;
        }

        /*
         * フォーカスするウィンドウを変更する
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


        public WindowInfoWithHandle MoveCurrentFocusTop()
        {
            this.CurrentWindowInfoIndex = 0;
            return this.GetCurrentWindow();
        }

        public WindowInfoWithHandle MoveCurrentFocusBottom()
        {
            this.CurrentWindowInfoIndex = this.WindowInfos.Count() - 1;
            return this.GetCurrentWindow();
        }

        /*a
         * アクティヴウィンドウの位置を移動する
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

        /*
         * ウィンドウをアクティヴにする
         */
        public void ActivateWindow(WindowInfoWithHandle windowInfoWithHandle)
        {
            SetForegroundWindow(windowInfoWithHandle.WindowHandle);
        }

        /*
         * src と dest のウィンドウの位置を入れ替える
         */
        public void ChangeWindowPosition(WindowInfoWithHandle srcWindowInfoWithHandle, 
            WindowInfoWithHandle destWindowInfoWithHandle)
        {
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
