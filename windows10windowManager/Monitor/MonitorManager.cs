using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using System.Diagnostics;

using windows10windowManager.Window;
using windows10windowManager.Util;

namespace windows10windowManager.Monitor
{

    class MonitorManager
    {

        #region Delegate
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor,
            ref RECT lprcMonitor, IntPtr dwData);
        #endregion

        #region WinApi
        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
            MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hmon, ref MONITORINFO mi);
        #endregion

        #region Fields
        public List<MonitorInfoWithHandle> monitorInfos { get; set; }

        public List<WindowManager> windowManagers { get; set; }

        protected int currentWindowManagerIndex { get; set; } = 0;
        #endregion

        public MonitorManager(TraceWindow traceWindow)
        {
            this.windowManagers = new List<WindowManager>();

            this.FindMonitors();

            // 渡された WindowHandles を渡し、モニターごとにWindowManagerで管理する
            this.ManageWindowByMonitors(traceWindow.GetWindowInfos());
        }

        /**
         * <summary>
         * 現在の環境の全てのモニターを取得し MonitorInfos に保持する
         * </summary>
         * <returns></returns>
         */
        public List<MonitorInfoWithHandle> FindMonitors()
        {
            // New List
            this.monitorInfos = new List<MonitorInfoWithHandle>();

            // Enumerate monitors
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, this.FindMonitorEnum, IntPtr.Zero);

            return this.monitorInfos;
        }

        public bool FindMonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var mi = new MONITORINFO();
            mi.size = (uint)Marshal.SizeOf(mi);
            GetMonitorInfo(hMonitor, ref mi);

            // Add to monitor info
            this.monitorInfos.Add(new MonitorInfoWithHandle(hMonitor, lprcMonitor, mi));
            return true;
        }

        /**
         * <summary>
         * 複数の WindowInfoWithHandleを各モニターごとに管理する
         * </summary>
         * <param name="windowInfoWithHandles"></param>
         */
        public void ManageWindowByMonitors(List<WindowInfoWithHandle> windowInfoWithHandles)
        {
            this.windowManagers.Clear();

            foreach (var monitorInfo in this.monitorInfos)
                {

                }

            for (int i = 0; i < this.monitorInfos.Count; i++)
            {
                var monitorInfo = this.monitorInfos[i];
                var monitorHandle = monitorInfo.monitorHandle;
                var monitorName = new string(monitorInfo.monitorInfo.szDevice).TrimEnd('\0');
                Logger.WriteLine($"Add WindowManager of Monitor : {monitorName} ({monitorHandle})");
                var windowManager = new WindowManager(monitorInfo.monitorHandle);
                windowManager.windowTilingType = (WindowTilingType)SettingManager.GetInt($"Window_WindowManager{i}_WindowTilingType");
                this.windowManagers.Add(windowManager);
            }

            foreach (var windowInfoWithHandle in windowInfoWithHandles)
            {
                this.AddWindowInfo(windowInfoWithHandle);
            }

            // 全画面を初期状態で整列する
            foreach ( var windowManager in this.windowManagers)
            {
                var monitorHandle = windowManager.monitorHandle;
                var monitorInfoWithHandle = this.FindMonitorInfoByMonitorHandle(monitorHandle);
                var windowTiler = new WindowTiler(
                    /* windowTilingType =  */ windowManager.windowTilingType,
                    /* windowCount =  */ windowManager.WindowCount(),
                    /* monitorRect = */ monitorInfoWithHandle.monitorInfo.work);
                windowManager.ArrangeWindows(windowTiler);
                windowManager.MoveCurrentFocusTop();
            }

            this.SetCurrentWindowManagerIndex(0);
            this.GetCurrentMonitorWindowManager().GetCurrentWindow().ActivateWindow();
        }

        /**
         * <summary>
         * カレントモニターの WindowManager を取得する
         * </summary> 
         */
        public WindowManager GetCurrentMonitorWindowManager()
        {
            return this.windowManagers.ElementAt(this.currentWindowManagerIndex);
        }

        /**
         * <summary>
         * カレントモニターの WindowManager のインデックスを戻す
         * </summary>
         */
        public int GetCurrentWindowManagerIndex()
        {
            return this.currentWindowManagerIndex;
        }

        public void SetCurrentWindowManagerIndex(int windowManagerIndex)
        {
            Logger.WriteLine($"MonitorManager.SetCurrentWindowManagerIndex = {windowManagerIndex}");
            this.currentWindowManagerIndex = windowManagerIndex;
        }

        /**
         * <summary>
         * カレントモニターの MonitorInfo を戻す
         * </summary>
         */
        public MonitorInfoWithHandle GetCurrentMonitor()
        {
            var monitorHandle = this.GetCurrentMonitorWindowManager().monitorHandle;
            return this.FindMonitorInfoByMonitorHandle(monitorHandle);
        }

        /**
         * <summary>
         * 現在のモニターのひとつ前のモニターのWindowManagerを戻す
         * </summary>
         */
        public WindowManager MoveCurrentFocusPrevious()
        {
            var previousWindowManagerIndex = this.GetCurrentWindowManagerIndex() - 1;
            if (previousWindowManagerIndex < 0)
            {
                previousWindowManagerIndex = this.windowManagers.Count - 1;
            }
            Logger.WriteLine($"MoveCurrentFocusPrevious = {previousWindowManagerIndex}");
            this.SetCurrentWindowManagerIndex(previousWindowManagerIndex);
            return this.GetCurrentMonitorWindowManager();
        }

        /**
         * <summary>
         * 現在のモニターのひとつ後ろのモニターのWindowManagerを戻す
         * </summary>
         */
        public WindowManager MoveCurrentFocusNext()
        {
            var nextWindowManagerIndex = this.GetCurrentWindowManagerIndex() + 1;
            if (nextWindowManagerIndex >= this.windowManagers.Count )
            {
                nextWindowManagerIndex = 0;
            }
            Logger.WriteLine($"MoveCurrentFocusNext = {nextWindowManagerIndex}");
            this.SetCurrentWindowManagerIndex(nextWindowManagerIndex);
            return this.GetCurrentMonitorWindowManager();
        }

        /**
         * <summary>
         * 指定されたモニターハンドルをカレントウィンドウマネージャーにする
         * </summary>
         */
        public void SetCurrentWindowManagerIndexByMonitorHandle(IntPtr monitorHandle)
        {
            var wm = this.FindWindowManagerByMonitorHandle(monitorHandle);
            this.SetCurrentWindowManagerIndex(this.windowManagers.IndexOf(wm));
        }

        /**
         * <summary>
         * this.WindowManagers のうち、指定された monitorNumber のモニター用に管理している
         * WindowManager を戻してこのモニターをアクティヴにする
         * </summary>
         */
        public WindowManager ActivateMonitorNWindowManager(int monitorNumber)
        {
            if (this.monitorInfos.Count < monitorNumber + 1)
            {
                return null;
            }
            var monitorInfo = this.monitorInfos.ElementAt(monitorNumber);
            Logger.DebugMonitor($"Get Monitor number={monitorNumber}", monitorInfo);
            this.currentWindowManagerIndex = monitorNumber;
            return this.FindWindowManagerByMonitorHandle(monitorInfo.monitorHandle);
        }

        /**
         * <summary>
         * 指定したモニターハンドルに紐付くWindowManagerを戻す
         * </summary>
         */
        public WindowManager FindWindowManagerByMonitorHandle(IntPtr monitorHandle)
        {
            return this.windowManagers.Find(
                (WindowManager wm) => { return wm.monitorHandle == monitorHandle; });
        }

        /**
         * <summary>
         * 指定したモニターハンドルに紐付く MonitorInfo を戻す
         * </summary>
         */
        public MonitorInfoWithHandle FindMonitorInfoByMonitorHandle(IntPtr monitorHandle)
        {
            return this.monitorInfos.Find(
                (MonitorInfoWithHandle mi) => { return mi.monitorHandle == monitorHandle; });
        }

        /**
         * <summary>
         * WindowInfoWithHandle をカレントモニター、カレントウィンドウにする
         * </summary>
         */
        public void ActivateWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(monitorHandle);
            if (targetWindowManager is null)
            {
                return;
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.ActivateWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.ActivateWindowInfo", windowInfoWithHandle);
            this.SetCurrentWindowManagerIndexByMonitorHandle(monitorHandle);
            targetWindowManager.SetCurrentWindowIndexByWindowInfo(windowInfoWithHandle);
            return;

        }

        /**
         * <summary>
         * WindowInfoWithHandle をモニターのWindowManagerの先頭に追加する
         * </summary>
         */
        public WindowManager PushNewWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(monitorHandle);
            if (targetWindowManager is null)
            {
                return this.GetCurrentMonitorWindowManager();
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.PushNewWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.PushNewWindowInfo", windowInfoWithHandle);
            this.SetCurrentWindowManagerIndexByMonitorHandle(monitorHandle);
            targetWindowManager.PushNew(windowInfoWithHandle);
            return targetWindowManager;
        }


        /**
         * <summary>
         * WindowInfoWithHandle をモニターのWindowManagerの先頭に追加する
         * </summary>
         */
        public WindowManager PushWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(monitorHandle);
            if (targetWindowManager is null)
            {
                return this.GetCurrentMonitorWindowManager();
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.PushWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.PushWindowInfo", windowInfoWithHandle);
            this.SetCurrentWindowManagerIndexByMonitorHandle(monitorHandle);
            targetWindowManager.Push(windowInfoWithHandle);
            return targetWindowManager;
        }

        /**
         * <summary>
         * WindowInfoWithHandle をモニターのWindowManagerに追加する
         * </summary>
         */
        public WindowManager AddWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(monitorHandle);
            if ( targetWindowManager is null)
            {
                return this.windowManagers.ElementAt(0);
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.AddWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.AddWindowInfo", windowInfoWithHandle);
            this.SetCurrentWindowManagerIndexByMonitorHandle(monitorHandle);
            targetWindowManager.Add(windowInfoWithHandle);
            return targetWindowManager;
        }

        /**
         * <summary>
         * WindowInfoWithHandle をモニターの WindowManager から削除する
         * </summary>
         */
        public WindowManager RemoveWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(windowInfoWithHandle.GetMonitorHandle());
            if (targetWindowManager is null)
            {
                return this.windowManagers.ElementAt(0);
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.RemoveWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.RemoveWindowInfo", windowInfoWithHandle);
            targetWindowManager.Remove(windowInfoWithHandle);
            return targetWindowManager;
        }

        /**
         * <summary>
         * 現在のアクティヴディスプレイをハイライト表示する
         * </summary>
         */
        public void HighlightCurrentMonitor()
        {
            var currentWindowManager = this.GetCurrentMonitorWindowManager();
            var currentMonitorInfo = this.monitorInfos.Find(
                (MonitorInfoWithHandle mi) => { return mi.monitorHandle == currentWindowManager.monitorHandle; });
            //currentMonitorInfo.MonitorInfo.

            Logger.DebugMonitor("Hightlight", currentMonitorInfo);
            MonitorInformationForm monitorInformationForm = new MonitorInformationForm(currentMonitorInfo);
            monitorInformationForm.Show();
        }


    }
}
