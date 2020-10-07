using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

using eventHook.EventMap;
using System.CodeDom;

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
        const UInt32 WS_VISIBLE = 0x10000000;
        const UInt32 WS_DISABLED = 0x08000000;
        const UInt32 WS_CAPTION = 0x00C00000;
        const UInt32 WS_OVERLAPPEDWINDOW = 0x00CF0000;

        // for GetWindow
        const uint GW_OWNER = 4;

        public class OriginalWinEventArg : EventArgs
        {
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        public delegate void WinEventHandler(object sender, OriginalWinEventArg e);
        //public event WinEventHandler FocusEvent;
        // public event WinEventHandler ResizeEvent;

        protected List<IntPtr> WindowHandles { get; set; } = new List<IntPtr>();

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
            /*
            if ((windowLong & WS_POPUP ) == WS_POPUP)
            {
                return;
            }
            */


            if (is_visible && is_overlappedwindow &&  eventName == EventName.EVENT_OBJECT_SHOW)
            //if (is_visible && eventName == EventName.EVENT_OBJECT_SHOW)
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
            else if ( eventName == EventName.EVENT_OBJECT_DESTROY)
            {
                if (WindowHandles.Contains(hwnd))
                {
                    Debug.WriteLine("Window destroyed: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Remove(hwnd);
                }
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
