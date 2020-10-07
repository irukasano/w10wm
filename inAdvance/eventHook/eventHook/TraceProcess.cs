﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

using eventHook.EventMap;
using System.CodeDom;
using System.Security.Principal;

namespace eventHook
{
    class TraceProcess : AbstractTraceProcess
    {
        const uint OBJID_WINDOW      = 0x00000000;
        const uint OBJID_SYSMENU = 0xFFFFFFFF;
        const uint OBJID_TITLEBAR = 0xFFFFFFFE;
        const uint OBJID_MENU = 0xFFFFFFFD;
        const uint OBJID_CLIENT = 0xFFFFFFFC;
        const uint OBJID_VSCROLL = 0xFFFFFFFB;
        const uint OBJID_HSCROLL = 0xFFFFFFFA;
        const uint OBJID_SIZEGRIP = 0xFFFFFFF9;
        const uint OBJID_CARET = 0xFFFFFFF8;
        const uint OBJID_CURSOR = 0xFFFFFFF7;
        const uint OBJID_ALERT = 0xFFFFFFF6;
        const uint OBJID_SOUND = 0xFFFFFFF5;

        // for GetWindowLong
        const int GWL_STYLE = (-16);
        const UInt32 WS_POPUP = 0x80000000;
        const UInt32 WS_CHILD = 0x40000000;
        const UInt32 WS_MINIMIZE = 0x20000000;
        const UInt32 WS_VISIBLE = 0x10000000;
        const UInt32 WS_DISABLED = 0x08000000;
        const UInt32 WS_CAPTION = 0x00C00000;
        const UInt32 WS_CLIPCHILDREN = 0x02000000;
        const UInt32 WS_OVERLAPPEDWINDOW = 0x00CF0000;

        // for GetWindow
        const uint GW_OWNER = 4;

        public class OriginalWinEventArg : EventArgs
        {
        }

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        private delegate bool EnumWindowsDelegate(IntPtr hwnd, IntPtr lParam);

        public delegate void WinEventHandler(object sender, OriginalWinEventArg e);
        //public event WinEventHandler FocusEvent;
        // public event WinEventHandler ResizeEvent;

        protected List<IntPtr> WindowHandles { get; set; } = new List<IntPtr>();
        protected IntPtr MouseDraggingWindowHandle { get; set; }

        public void InitializeWindowHandles()
        {
            EnumWindows(EnumerateWindows, IntPtr.Zero);
        }

        private  bool EnumerateWindows(IntPtr hwnd, IntPtr lParam)
        {
            var windowLong = GetWindowLong(hwnd, GWL_STYLE);
            var windowTitle = GetCurrentWindowTitle(hwnd);
            var is_visible = (windowLong & WS_VISIBLE) == WS_VISIBLE;
            var is_overlappedwindow = (windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW;
            var is_minimized = (windowLong & WS_MINIMIZE) == WS_MINIMIZE;
            var is_clipchildren = (windowLong & WS_CLIPCHILDREN) == WS_CLIPCHILDREN;

            /*
             * Biscute.exe は以下の状態なので、WS_OVERLAPPEDWINDOW - WS_SYSMENU なのかも
             * Window add: ********* : 15c70000
             */

            //Debug.WriteLine("Window add: " + windowTitle + " : " + windowLong.ToString("x8"));

            if (!WindowHandles.Contains(hwnd))
            {
                if (is_visible && is_overlappedwindow && is_clipchildren && !is_minimized && windowTitle != null)
                {
                    Debug.WriteLine("Window add: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Add(hwnd);
                }
            }

            return true;
        }

        public override void HookProcedure(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var eventName = EventMapConverter.CodeToName((int)eventType);

            if ((uint)idObject != OBJID_WINDOW)
            {
                return;
            }
            if (eventName == EventName.UNKNOWN)
            {
                return;
            }
            var windowLong = GetWindowLong(hwnd, GWL_STYLE);
            var windowTitle = GetCurrentWindowTitle(hwnd);
            var is_visible = (windowLong & WS_VISIBLE) == WS_VISIBLE;
            var is_overlappedwindow = (windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW;

            //Debug.WriteLine( windowTitle + " : " + windowLong.ToString("x8"));

            if ( windowLong == 0)
            {
                return;
            }
            if ((windowLong & WS_CHILD) == WS_CHILD)
            {
                return;
            }


            if (! WindowHandles.Contains(hwnd))
            {
                if (is_visible && is_overlappedwindow && 
                    (eventName == EventName.EVENT_OBJECT_SHOW || 
                    eventName == EventName.EVENT_OBJECT_NAMECHANGE))
                {
                    Debug.WriteLine("Window created: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Add(hwnd);
                    /*
                    Debug.WriteLine("---");
                    Debug.WriteLine(windowTitle + ":" + is_visible + ":" + eventName + ":" + windowLong);
                    Debug.WriteLine("WS_VISIBLE = " + ((windowLong & WS_VISIBLE) == WS_VISIBLE));
                    Debug.WriteLine("WS_OVERLAPPEDWINDOW = " + ((windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW));
                    */
                }

            } else
            {
                if (eventName == EventName.EVENT_OBJECT_DESTROY)
                {
                    // Window がなくなった
                    Debug.WriteLine("Window destroyed: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Remove(hwnd);
                }
                else if (eventName == EventName.EVENT_OBJECT_HIDE)
                {
                    // Window が HIDDEN
                    Debug.WriteLine("Window hide: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Remove(hwnd);
                }
                else if (eventName == EventName.EVENT_SYSTEM_MOVESIZESTART)
                {
                    // Window がマウスでドラッグ開始
                    Debug.WriteLine("Window mouse drag start: " + windowTitle + " : " + windowLong.ToString("x8"));
                    MouseDraggingWindowHandle = hwnd;
                } 
                else if ( eventName == EventName.EVENT_SYSTEM_MOVESIZEEND)
                {
                    // Window がマウスでドラッグ終了
                    Debug.WriteLine("Window mouse drag end: " + windowTitle + " : " + windowLong.ToString("x8"));
                    MouseDraggingWindowHandle = IntPtr.Zero;
                }
                else if ( eventName == EventName.EVENT_OBJECT_LOCATIONCHANGE)
                {
                    // ショートカットキーだけで場所移動
                    if (MouseDraggingWindowHandle == IntPtr.Zero)
                    {
                        Debug.WriteLine("Window location change: " + windowTitle + " : " + windowLong.ToString("x8"));
                    }
                }
            }

            if (WindowHandles.Contains(hwnd))
            {
                // Debug.WriteLine(" - " + eventName + " : " + windowTitle);

            }

            /*
            Debug.WriteLine("----");
            Debug.WriteLine("hWinEventHook:" + hWinEventHook);
            Debug.WriteLine("eventType:"+eventName);
            Debug.WriteLine("hwnd:" + hwnd);
            Debug.WriteLine("title:" + GetCurrentWindowTitle(hwnd));
            Debug.WriteLine("idObject:" + idObject);
            Debug.WriteLine("idChild:" + idChild);
            Debug.WriteLine("dwEventThread:" + dwEventThread);
            Debug.WriteLine("dwmsEventTime:" + dwmsEventTime);
            */
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

    }
}
