using System;
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

        public WindowManager(IntPtr monitorHandle)
        {
            this.MonitorHandle = monitorHandle;
            this.WindowInfos = new List<WindowInfoWithHandle>();
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
            return WindowInfos.First();
        }

        // カレントウィンドウの WindowInfos 内のインデックスを取得する
        public int GetCurrentWindowIndex()
        {
            return CurrentWindowInfoIndex;
        }

        /*
        // ひとつ上のウィンドウを取得する
        public WindowInfoWithHandle GetPreviousWindow()
        {
        }
        */

        // GetNextWindow();
        // GetTopWindow();
        // GetBottomWindow();

        // SetWindowPrevious()
        // SetWindowNext
        // SetWindowTop
        // SetWindowBottom
        public void SetWindowBottom()
        {
            var currentIndex = GetCurrentWindowIndex();
            var bottomIndex = WindowInfos.Count() - 1;
            if ( currentIndex != bottomIndex)
            {
                Move(WindowInfos, currentIndex, bottomIndex);
                // TODO 再描画
            }
        }

        protected void Move<T>(List<T> list, int oldIndex, int newIndex)
        {
            T aux = list[newIndex];
            list[newIndex] = list[oldIndex];
            list[oldIndex] = aux;
        }

        // ウィンドウをアクティヴにする
        // ActivateWindow(WindowInfoWithHandle);
    }
}
