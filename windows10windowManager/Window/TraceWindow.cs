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

using windows10windowManager.Util;

namespace windows10windowManager.Window
{
    class TraceWindow : AbstractTraceWindow
    {
        #region Constraint
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
        #endregion

        public class OriginalWinEventArg : EventArgs
        {
            public WindowInfoWithHandle WindowInfo { get; }

            public OriginalWinEventArg(WindowInfoWithHandle windowInfo)
            {
                this.WindowInfo = windowInfo;
            }
        }

        #region WinApi
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
        const UInt32 WS_TYPE_VSCODE = 0x00C70000;
        const UInt32 WS_UWP = WS_POPUP | WS_CLIPSIBLINGS | WS_OVERLAPPEDWINDOW;

        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

        private delegate bool EnumWindowsDelegate(IntPtr hwnd, IntPtr lParam);
        #endregion

        #region Delegate
        public delegate void EventHandler(object sender, OriginalWinEventArg w);
        public event EventHandler AddEvent;
        public event EventHandler RemoveEvent;
        public event EventHandler ShowEvent;
        public event EventHandler DestroyEvent;
        public event EventHandler HideEvent;
        public event EventHandler MouseDragStartEvent;
        public event EventHandler MouseDragEndEvent;
        public event EventHandler LocationChangeEvent;
        #endregion

        #region Field
        public List<WindowInfoWithHandle> WindowInfos { get; protected set; } = new List<WindowInfoWithHandle>();
        protected WindowInfoWithHandle MouseDraggingWindowHandle { get; set; }
        #endregion

        public TraceWindow()
        {
            EnumWindows(EnumerateWindows, IntPtr.Zero);
        }

        private bool EnumerateWindows(IntPtr hWnd, IntPtr lParam)
        {
            var windowInfo = new WindowInfoWithHandle(hWnd);
            var windowLong = GetWindowLong(hWnd, GWL_STYLE);
            var windowTitle = windowInfo.windowTitle;

            if (!this.WindowInfos.Contains(windowInfo))
            {
                if ( this.IsValidWindow(hWnd, windowLong, windowTitle))
                {
                    Logger.WriteLine("TraceWindows.EnumerateWindows ADD:  : {windowTitle} : {windowLongString}");
                    this.WindowInfos.Add(windowInfo);
                    OnAddEvent(windowInfo);
                }
            }

            return true;
        }

        /**
         * <summary>
         * 対象のウィンドウが本アプリケーションの管理下に置かれるべきウィンドウかどうか判定する
         * </summary>
         */
        protected bool IsValidWindow(IntPtr hWnd, UInt32 windowLong, string windowTitle)
        {
            var isVisible = (windowLong & WS_VISIBLE) == WS_VISIBLE;
            var isOverlappedwindow = (windowLong & WS_OVERLAPPEDWINDOW) == WS_OVERLAPPEDWINDOW;
            var isTypeVscode = (windowLong & WS_TYPE_VSCODE) == WS_TYPE_VSCODE;
            var isUwp = (windowLong & WS_UWP) == WS_UWP;

            //var isMinimized = (windowLong & WS_MINIMIZE) == WS_MINIMIZE;
            //var isClipchildren = (windowLong & WS_POPUP & WS_CLIPCHILDREN) == (WS_POPUP & WS_CLIPCHILDREN);
            //DwmGetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.Cloaked, out var isCloaked, Marshal.SizeOf(typeof(bool)));

            /*
             * * Biscute.exe
             * TraceWindows.EnumerateWindows : (1) 受信箱 | **** | ProtonMail : 14030000
             * 
             * * Microsoft Code
             * README.md - Visual Studio Code : 14c70000
             * 
             * * Chrome
             * ***** - Chromium : 16cf0000
             * 
             * * Edge
             * **** - Microsoft? Edge : 17cf0000
             * 
             * * AS/R
             * w10wm - Asr : 14cf4000
             */

            var windowLongString = windowLong.ToString("x8");
            Logger.WriteLine($"TraceWindows.IsValidWindow : ({hWnd}){windowTitle} : {windowLongString}");

            return (isVisible && 
                (isOverlappedwindow || isTypeVscode) && 
                !isUwp && 
                windowTitle != null);
        }

        public override void HookProcedure(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
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
            var needleWindowInfo = new WindowInfoWithHandle(hWnd);
            var windowLong = GetWindowLong(hWnd, GWL_STYLE);
            var windowLongString = windowLong.ToString("x8");
            var windowTitle = needleWindowInfo.windowTitle;

            if ( windowLong == 0 ||
                ((windowLong & WS_POPUP) == WS_POPUP) ||
                ((windowLong & WS_CHILD) == WS_CHILD))
            {
                return;
            }

            if (! this.WindowInfos.Contains(needleWindowInfo))
            {
                if (this.IsValidWindow(hWnd, windowLong, windowTitle) &&
                    (eventName == EventName.EVENT_OBJECT_SHOW || 
                    eventName == EventName.EVENT_OBJECT_NAMECHANGE))
                {
                    Logger.WriteLine($"TraceWindows.HookProcedure OnAdd : ({hWnd}){windowTitle} : {windowLongString}");
                    this.WindowInfos.Add(needleWindowInfo);
                    OnAddEvent(needleWindowInfo);
                    OnShowEvent(needleWindowInfo);
                }

            } else
            {
                var windowInfo = this.WindowInfos.Find(
                    (WindowInfoWithHandle wi) => { return wi.windowHandle == needleWindowInfo.windowHandle; });

                if (eventName == EventName.EVENT_OBJECT_DESTROY)
                {
                    // Window がなくなった
                    Logger.WriteLine($"TraceWindows.HookProcedure OnDestroy : ({hWnd}){windowTitle} : {windowLongString}");
                    this.WindowInfos.Remove(windowInfo);
                    OnRemoveEvent(windowInfo);
                    OnDestroyEvent(windowInfo);
                }
                else if (eventName == EventName.EVENT_OBJECT_HIDE)
                {
                    // Window が HIDDEN
                    Logger.WriteLine($"TraceWindows.HookProcedure OnHide : ({hWnd}){windowTitle} : {windowLongString}");
                    this.WindowInfos.Remove(windowInfo);
                    OnRemoveEvent(windowInfo);
                    OnHideEvent(windowInfo);
                }
                else if (eventName == EventName.EVENT_SYSTEM_MOVESIZESTART)
                {
                    // Window がマウスでドラッグ開始
                    Logger.WriteLine($"TraceWindows.HookProcedure OnMouseDragStart : ({hWnd}){windowTitle} : {windowLongString}");
                    MouseDraggingWindowHandle = windowInfo;
                    OnMouseDragStartEvent(windowInfo);
                } 
                else if ( eventName == EventName.EVENT_SYSTEM_MOVESIZEEND)
                {
                    // Window がマウスでドラッグ終了
                    if (MouseDraggingWindowHandle.Equals(windowInfo))
                    {
                        Logger.WriteLine($"TraceWindows.HookProcedure OnMouseDragEnd : ({hWnd}){windowTitle} : {windowLongString}");
                        MouseDraggingWindowHandle = null;
                        OnMouseDragEndEvent(windowInfo);
                        OnLocationChangeEvent(windowInfo);
                    }
                }
                else if ( eventName == EventName.EVENT_OBJECT_LOCATIONCHANGE)
                {
                    // ショートカットキーだけで場所移動
                    if (MouseDraggingWindowHandle == null)
                    {
                        Logger.WriteLine($"TraceWindows.HookProcedure OnLocationChange : ({hWnd}){windowTitle} : {windowLongString}");
                        OnLocationChangeEvent(windowInfo);
                    }
                }
            }

            /*
            if (this.WindowInfos.Contains(needleWindowInfo))
            {
                // Debug.WriteLine(" - " + eventName + " : " + windowTitle);

            }

            Debug.WriteLine("----");
            Debug.WriteLine("hWinEventHook:" + hWinEventHook);
            Debug.WriteLine("eventType:"+eventName);
            Debug.WriteLine("hwnd:" + hwnd);
            Debug.WriteLine("title:" + GetCurrentWindowTitle(hwnd));0
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
