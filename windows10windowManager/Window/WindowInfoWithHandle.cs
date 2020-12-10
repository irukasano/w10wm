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


        public IntPtr windowHandle { get; private set; }

        public RECT position { get; private set; }

        public String windowTitle { get; private set; }

        public IntPtr monitorHandle { get; private set; }

        public WindowInfoWithHandle(IntPtr hWnd)
        {
            this.windowHandle = hWnd;

            RECT rect;
            bool f = GetWindowRect(hWnd, out rect);
            this.position = rect;

            this.windowTitle = GetCurrentWindowTitle(hWnd);

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
            if (! this.windowHandle.Equals(IntPtr.Zero))
            {
                SendMessage(this.windowHandle,
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
            SetForegroundWindow(this.windowHandle);
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
