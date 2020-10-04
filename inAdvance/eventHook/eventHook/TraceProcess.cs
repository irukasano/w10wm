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

        public class OriginalWinEventArg : EventArgs
        {
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public delegate void WinEventHandler(object sender, OriginalWinEventArg e);
        //public event WinEventHandler FocusEvent;
        // public event WinEventHandler ResizeEvent;

        public override void HookProcedure(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var eventName = EventMapConverter.CodeToName((int)eventType);

            if (eventName == EventName.UNKNOWN)
            {
                return;
            }
            if (eventName == EventName.EVENT_OBJECT_LOCATIONCHANGE)
            {
                return;
            }
            /*
            if ( eventName == EventName.EVENT_OBJECT_VALUECHANGE)
            {
                return;
            }
            if (eventName == EventName.EVENT_OBJECT_REORDER)
            {
                return;
            }
            */

            /*
            if (idChild != 0)
            {
                return;
            }
            */
            if ((uint)idObject != OBJID_ALERT &&
                (uint)idObject != OBJID_WINDOW)
            {
                return;
            }


            Debug.WriteLine("----");
            Debug.WriteLine("hWinEventHook:" + hWinEventHook);
            Debug.WriteLine("eventType:"+eventName);
            Debug.WriteLine("hwnd:" + hwnd);
            Debug.WriteLine("title:" + GetCurrentWindowTitle(hwnd));
            Debug.WriteLine("idObject:" + idObject);
            Debug.WriteLine("idChild:" + idChild);
            Debug.WriteLine("dwEventThread:" + dwEventThread);
            Debug.WriteLine("dwmsEventTime:" + dwmsEventTime);
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
