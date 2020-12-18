using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace eventHook
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
        IntPtr WindowHandle { get; }
        RECT Position { get; }
    }

    public class WindowInfoWithHandle : IWindowInfoWithHandle, IEquatable<WindowInfoWithHandle>
    {
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public IntPtr WindowHandle { get; private set; }
        public RECT Position { get; private set; }

        public String WindowTitle { get; private set; }

        public WindowInfoWithHandle(IntPtr hwnd)
        {
            WindowHandle = hwnd;

            RECT rect;
            bool f = GetWindowRect(hwnd, out rect);
            Position = rect;

            WindowTitle = GetCurrentWindowTitle(hwnd);
        }

        public bool Equals( WindowInfoWithHandle other)
        {
            return this.WindowHandle == other.WindowHandle;
        }

        private string GetCurrentWindowTitle(IntPtr hwnd)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(hwnd, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public bool MovedMonitor( IntPtr hwnd)
        {
            // TODO たぶんMonitorManager あたりで判定する必要がある？
            RECT rect;
            bool f = GetWindowRect(hwnd, out rect);
            // return MonitorManager.IsDifferentMonitor(Position, rect);
            return true;
        }

    }
}
