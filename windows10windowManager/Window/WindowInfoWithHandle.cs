using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using windows10windowManager.Util;

namespace windows10windowManager.Window
{

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public interface IWindowInfoWithHandle
    {
        IntPtr windowHandle { get; }
        RECT position { get; }
    }

    public class WindowInfoWithHandle : IWindowInfoWithHandle, IEquatable<WindowInfoWithHandle>
    {
        // MonitorFromWindow
        const int MONITOR_DEFAULTTONULL = 0;
        const int MONITOR_DEFAULTTOPRIMARY = 1;
        const int MONITOR_DEFAULTTONEAREST = 2;

        #region WinApi
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg, UIntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(
            IntPtr hWnd, IntPtr ProcessId);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AttachThreadInput(
            uint idAttach, uint idAttachTo, bool fAttach);
        
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(
            IntPtr hWnd, out int lpdwProcessId);
        #endregion

        #region Field
        public IntPtr windowHandle { get; private set; }

        public RECT position { get; private set; }

        public String windowTitle { get; private set; }

        public IntPtr monitorHandle { get; private set; }

        public int processId { get; private set; }
        #endregion

        public WindowInfoWithHandle(IntPtr hWnd)
        {
            this.windowHandle = hWnd;

            RECT rect;
            bool f = GetWindowRect(hWnd, out rect);
            this.position = rect;

            this.windowTitle = GetCurrentWindowTitle(hWnd);

            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            this.processId = processId;

            this.ComputeMonitorHandle();
        }

        public bool Equals( WindowInfoWithHandle other)
        {
            return this.windowHandle == other.windowHandle;
        }

        private string GetCurrentWindowTitle(IntPtr hWnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(hWnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        /**
         * <summary>
         * このウィンドウの現在位置情報を WindowRect で戻す
         * </summary>
         */
        public WindowRect GetCurrentWindowRect()
        {
            RECT rect;
            bool f = GetWindowRect(this.windowHandle, out rect);
            return new WindowRect(rect.top, rect.bottom, rect.left, rect.right);
        }

        /**
         * <summary>
         * このウィンドウの初期表示時の位置を WindowRect で戻す
         * </summary>
         */
        public WindowRect GetOriginalWindowRect()
        {
            var rect = this.position;
            return new WindowRect(rect.top, rect.bottom, rect.left, rect.right);
        }

        /**
         * <summary>
         * ウィンドウを閉じる
         * </summary>
         */
        public void WindowClose()
        {
            var hWnd = this.windowHandle;
            Logger.WriteLine($"WindowInfoWithHandle.ActivateWindow : hWnd={hWnd}");
            if (! hWnd.Equals(IntPtr.Zero))
            {
                SendMessage(hWnd,
                    /* wMsg = WM_SYSCOMMAND */ 0x0112,
                    /* wParam = */ new UIntPtr(0x0000F060),
                    /* lParam = */ IntPtr.Zero
                );
                //WindowHandle = IntPtr.Zero;
            }
        }

        /**
         * <summary>
         * ウィンドウをアクティヴにする
         * </summary>
         */
        public void ActivateWindow()
        {
            var hWnd = this.windowHandle;
            Logger.WriteLine($"WindowInfoWithHandle.ActivateWindow : hWnd={hWnd}");
            if ( hWnd.Equals(IntPtr.Zero))
            {
                return;
            }
            if (IsIconic(hWnd))
            {
                return;
            }
            IntPtr forehWnd = GetForegroundWindow();
            if (forehWnd == hWnd)
            {
                return;
            }
            //フォアグラウンドのスレッドIDを取得
            uint foreThread = GetWindowThreadProcessId(forehWnd, IntPtr.Zero);
            //自分のスレッドIDを収得
            uint thisThread = GetCurrentThreadId();
            if (foreThread != thisThread)
            {
                AttachThreadInput(thisThread, foreThread, true);
            }

            SetForegroundWindow(this.windowHandle);
            BringWindowToTop(hWnd);

            if (foreThread != thisThread)
            {
                AttachThreadInput(thisThread, foreThread, false);
            }

            //SetForegroundWindow(this.windowHandle);

            //SetWindowPos(hWnd,
            //    /* HWND_TOPMOST */ -1,
            //0, 0, 0, 0,
            ///* SWP_NOMOVE | SWP_NOSIZE */
            //0x0002 | 0x0001);
            //SetWindowPos(hWnd,
            ///* HWND_NOTOPMOST */
            //-2,
            //0, 0, 0, 0,
            ///* SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE */
            //0x0040 | 0x0002 | 0x0001);

        }

        public IntPtr ComputeMonitorHandle()
        {
            this.monitorHandle = MonitorFromWindow(this.windowHandle, MONITOR_DEFAULTTONEAREST);

            return this.monitorHandle;
        }

        public IntPtr GetMonitorHandle()
        {
            return this.monitorHandle;
        }

        /**
         * <summary>
         * このウィンドウがモニターを移動したかを判定する
         * </summary>
         */
        public bool MovedMonitor()
        {
            var beforeMovedMonitorHandle = this.monitorHandle;
            var currentMonitorHandle = MonitorFromWindow(this.windowHandle, MONITOR_DEFAULTTONEAREST);

            return beforeMovedMonitorHandle != currentMonitorHandle;
        }

    }
}
