using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;

using windows10windowManager.Window.EventMap;

namespace windows10windowManager.Window
{
    class AbstractTraceWindow
    {
        const int WINEVENT_OUTOFCONTEXT = 0;
        const int WINEVENT_SKIPOWNTHREAD = 1;
        const int WINEVENT_SKIPOWNPROCESS = 2;
        const int WINEVENT_INCONTEXT = 4;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWinEventHook(int eventMin, int eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, int idProcess, int idThread, int dwflags);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnhookWinEvent(IntPtr hWinEventHook);

        #region Delegate
        private delegate void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        #endregion

        #region Fields
        private WinEventProc proc;
        private IntPtr hookId = IntPtr.Zero;
        #endregion

        public void Hook()
        {
            if (hookId == IntPtr.Zero)
            {
                proc = HookProcedure;
                hookId = SetWinEventHook(
                    EventMapConverter.NameToCode(EventName.EVENT_MIN), // eventMin
                    EventMapConverter.NameToCode(EventName.EVENT_MAX), // eventMax
                    IntPtr.Zero,             // hmodWinEventProc
                    proc,                    // lpfnWinEventProc
                    0,                       // idProcess
                    0,                       // idThread
                    WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

                if (hookId == IntPtr.Zero)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        public void UnHook()
        {
            UnhookWinEvent(hookId);
            hookId = IntPtr.Zero;
        }

        public virtual void HookProcedure(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            return;
        }
            
    }
}
