﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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
        IntPtr WindowHandle { get; }
        RECT Position { get; }
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

        public IntPtr WindowHandle { get; private set; }

        public RECT Position { get; private set; }

        public String WindowTitle { get; private set; }

        public IntPtr MonitorHandle { get; private set; }

        public WindowInfoWithHandle(IntPtr hwnd)
        {
            this.WindowHandle = hwnd;

            RECT rect;
            bool f = GetWindowRect(hwnd, out rect);
            this.Position = rect;

            this.WindowTitle = GetCurrentWindowTitle(hwnd);
        }

        public bool Equals( WindowInfoWithHandle other)
        {
            return this.WindowHandle == other.WindowHandle;
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

        public void WindowClose()
        {
            if (! WindowHandle.Equals(IntPtr.Zero))
            {
                SendMessage(this.WindowHandle,
                    /* wMsg = WM_SYSCOMMAND */ 0x0112,
                    /* wParam = */ new UIntPtr(0x0000F060),
                    /* lParam = */ IntPtr.Zero
                );
                //WindowHandle = IntPtr.Zero;
            }
        }

        /* 
         * RECT を元に X,Y,Width,Height を求める処理
         */
        public int CalcPositionX(RECT position)
        {
            return position.left;
        }

        public int CalcPositionY(RECT position)
        {
            return position.top;
        }

        public int CalcPositionWidth(RECT position)
        {
            return position.right - position.left;
        }

        public int CalcPositionHeight(RECT position)
        {
            return position.bottom - position.top;
        }

        /*
         * このウィンドウの位置情報を戻す
         */
        public int GetPositionX()
        {
            return CalcPositionX(this.Position);
        }

        public int GetPositionY()
        {
            return CalcPositionY(this.Position);
        }

        public int GetPositionWidth()
        {
            return CalcPositionWidth(this.Position);
        }

        public int GetPositionHeight()
        {
            return CalcPositionHeight(this.Position);
        }

        public IntPtr GetMonitor()
        {
            this.MonitorHandle = MonitorFromWindow(this.WindowHandle, MONITOR_DEFAULTTONEAREST);

            return this.MonitorHandle;
        }

        public bool MovedMonitor( IntPtr hWnd)
        {
            // TODO たぶんMonitorManager あたりで判定する必要がある？
            RECT rect;
            bool f = GetWindowRect(hWnd, out rect);
            // return MonitorManager.IsDifferentMonitor(Position, rect);
            return true;
        }

    }
}
