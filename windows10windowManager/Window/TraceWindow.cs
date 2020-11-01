using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

using windows10windowManager.Window.EventMap;
using System.CodeDom;
using System.Security.Principal;

namespace windows10windowManager.Window
{
    class TraceWindow : AbstractTraceWindow
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
        const UInt32 WS_CLIPSIBLINGS = 0x04000000;
        const UInt32 WS_CAPTION = 0x00C00000;
        const UInt32 WS_CLIPCHILDREN = 0x02000000;
        const UInt32 WS_OVERLAPPEDWINDOW = 0x00CF0000;
        const UInt32 WS_UWP = WS_POPUP | WS_CLIPSIBLINGS | WS_OVERLAPPEDWINDOW;

        public class OriginalWinEventArg : EventArgs
        {
            public WindowInfoWithHandle WindowInfo { get; }

            public OriginalWinEventArg(WindowInfoWithHandle windowInfo)
            {
                WindowInfo = windowInfo;
            }
        }

        // DwmGetWindowAttribute
        enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            ExceludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }

        // for GetWindow
        const uint GW_OWNER = 4;

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

        private delegate bool EnumWindowsDelegate(IntPtr hwnd, IntPtr lParam);

        public delegate void EventHandler(object sender, OriginalWinEventArg w);
        public event EventHandler AddEvent;
        public event EventHandler RemoveEvent;
        public event EventHandler ShowEvent;
        public event EventHandler DestroyEvent;
        public event EventHandler HideEvent;
        public event EventHandler MouseDragStartEvent;
        public event EventHandler MouseDragEndEvent;
        public event EventHandler LocationChangeEvent;


        public List<WindowInfoWithHandle> WindowHandles { get; protected set; } = new List<WindowInfoWithHandle>();
        protected WindowInfoWithHandle MouseDraggingWindowHandle { get; set; }

        public TraceWindow()
        {
            EnumWindows(EnumerateWindows, IntPtr.Zero);
        }


        private bool EnumerateWindows(IntPtr hwnd, IntPtr lParam)
        {
            var windowInfo = new WindowInfoWithHandle(hwnd);
            var windowLong = GetWindowLong(hwnd, GWL_STYLE);
            var windowTitle = windowInfo.WindowTitle;
            var isVisible = (windowLong & WS_VISIBLE) == WS_VISIBLE;
            var isOverlappedwindow = (windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW;
            var isUwp = (windowLong & WS_UWP) == WS_UWP;

            //var isMinimized = (windowLong & WS_MINIMIZE) == WS_MINIMIZE;
            //var isClipchildren = (windowLong & WS_POPUP & WS_CLIPCHILDREN) == (WS_POPUP & WS_CLIPCHILDREN);

            //DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.Cloaked, out var isCloaked, Marshal.SizeOf(typeof(bool)));

            /*
             * Biscute.exe は以下の状態なので、WS_OVERLAPPEDWINDOW - WS_SYSMENU なのかも
             * Window add: ********* : 15c70000
             */

            //Debug.WriteLine("Window add: " + windowTitle + " : " + windowLong.ToString("x8"));

            if (!WindowHandles.Contains(windowInfo))
            {
                //if (isVisible && isOverlappedwindow && isClipchildren && !isMinimized && windowTitle != null)
                //if (isVisible && isOverlappedwindow  && !isCloaked && windowTitle != null)
                if (isVisible && isOverlappedwindow && !isUwp &&  windowTitle != null)
                //if (isVisible && isOverlappedwindow && windowTitle != null)
                {   
                    //Debug.WriteLine("Window add: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Add(windowInfo);
                    OnAddEvent(windowInfo);
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
            var windowInfo = new WindowInfoWithHandle(hwnd);
            var windowLong = GetWindowLong(hwnd, GWL_STYLE);
            var windowTitle = windowInfo.WindowTitle;
            var isVisible = (windowLong & WS_VISIBLE) == WS_VISIBLE;
            var isOverlappedwindow = (windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW;
            var isUwp = (windowLong & WS_UWP) == WS_UWP;

            if ( windowLong == 0)
            {
                return;
            }
            if ((windowLong & WS_CHILD) == WS_CHILD)
            {
                return;
            }


            if (! WindowHandles.Contains(windowInfo))
            {
                if (isVisible && isOverlappedwindow && !isUwp &&
                    (eventName == EventName.EVENT_OBJECT_SHOW || 
                    eventName == EventName.EVENT_OBJECT_NAMECHANGE))
                {
                    //Debug.WriteLine("Window created: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Add(windowInfo);
                    OnAddEvent(windowInfo);
                    OnShowEvent(windowInfo);
                    /*
                    Debug.WriteLine("---");
                    Debug.WriteLine(windowTitle + ":" + is_visible + ":" + eventName + ":" + windowLong);
                    Debug.WriteLine("WS_VISIBLE = " + ((windowLong & WS_VISIBLE) == WS_VISIBLE));
                    Debug.WriteLine("WS_OVERLAPPEDWINDOW = " + ((windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW));
                    */
                }

            } else
            {
                //Debug.WriteLine( " - " + windowTitle + " : " + eventName + ": " + windowLong.ToString("x8"));

                if (eventName == EventName.EVENT_OBJECT_DESTROY)
                {
                    // Window がなくなった
                    //Debug.WriteLine("Window destroyed: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Remove(windowInfo);
                    OnRemoveEvent(windowInfo);
                    OnDestroyEvent(windowInfo);
                }
                else if (eventName == EventName.EVENT_OBJECT_HIDE)
                {
                    // Window が HIDDEN
                    //Debug.WriteLine("Window hide: " + windowTitle + " : " + windowLong.ToString("x8"));
                    WindowHandles.Remove(windowInfo);
                    OnRemoveEvent(windowInfo);
                    OnHideEvent(windowInfo);
                }
                else if (eventName == EventName.EVENT_SYSTEM_MOVESIZESTART)
                {
                    // Window がマウスでドラッグ開始
                    //Debug.WriteLine("Window mouse drag start: " + windowTitle + " : " + windowLong.ToString("x8"));
                    MouseDraggingWindowHandle = windowInfo;
                    OnMouseDragStartEvent(windowInfo);
                } 
                else if ( eventName == EventName.EVENT_SYSTEM_MOVESIZEEND)
                {
                    // Window がマウスでドラッグ終了
                    if (MouseDraggingWindowHandle.Equals(windowInfo))
                    {
                        //Debug.WriteLine("Window mouse drag end: " + windowTitle + " : " + windowLong.ToString("x8"));
                        MouseDraggingWindowHandle = null;
                        OnMouseDragEndEvent(windowInfo);
                    }
                }
                else if ( eventName == EventName.EVENT_OBJECT_LOCATIONCHANGE)
                {
                    // ショートカットキーだけで場所移動
                    if (MouseDraggingWindowHandle == null)
                    {
                        //Debug.WriteLine("Window location change: " + windowTitle + " : " + windowLong.ToString("x8"));
                        OnLocationChangeEvent(windowInfo);
                    }
                }
            }

            if (WindowHandles.Contains(windowInfo))
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

        protected void OnAddEvent(WindowInfoWithHandle windowInfo)
        {
            AddEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnRemoveEvent(WindowInfoWithHandle windowInfo)
        {
            RemoveEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnShowEvent(WindowInfoWithHandle windowInfo)
        {
            ShowEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnDestroyEvent(WindowInfoWithHandle windowInfo)
        {
            DestroyEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnHideEvent(WindowInfoWithHandle windowInfo)
        {
            HideEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnMouseDragStartEvent(WindowInfoWithHandle windowInfo)
        {
            MouseDragStartEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnMouseDragEndEvent(WindowInfoWithHandle windowInfo)
        {
            MouseDragEndEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }
        protected void OnLocationChangeEvent(WindowInfoWithHandle windowInfo)
        {
            LocationChangeEvent?.Invoke(this, new OriginalWinEventArg(windowInfo));
        }

    }
}
