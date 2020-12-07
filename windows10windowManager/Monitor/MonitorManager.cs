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

        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor,
            ref RECT lprcMonitor, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
            MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hmon, ref MONITORINFO mi);

        public List<MonitorInfoWithHandle> MonitorInfos { get; set; }

        public List<WindowManager> WindowManagers { get; set; }

        protected int CurrentWindowManagerIndex { get; set; } = 0;


        public MonitorManager(TraceWindow traceWindow)
        {
            this.WindowManagers = new List<WindowManager>();

            this.FindMonitors();

            // 渡された WindowHandles を渡し、モニターごとにWindowManagerで管理する
            this.ManageWindowByMonitors(traceWindow.WindowInfos);
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
            this.MonitorInfos = new List<MonitorInfoWithHandle>();

            // Enumerate monitors
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, this.FindMonitorEnum, IntPtr.Zero);

            return this.MonitorInfos;
        }

        public bool FindMonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var mi = new MONITORINFO();
            mi.size = (uint)Marshal.SizeOf(mi);
            GetMonitorInfo(hMonitor, ref mi);

            // Add to monitor info
            MonitorInfos.Add(new MonitorInfoWithHandle(hMonitor, lprcMonitor, mi));
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
            this.WindowManagers.Clear();

            foreach (var monitorInfo in MonitorInfos)
            {
                var monitorHandle = monitorInfo.MonitorHandle;
                var monitorName = new string(monitorInfo.MonitorInfo.szDevice).TrimEnd('\0');
                Logger.WriteLine($"Add WindowManager of Monitor : {monitorName} ({monitorHandle})");
                this.WindowManagers.Add(new WindowManager(monitorInfo.MonitorHandle));
            }

            foreach (var windowInfoWithHandle in windowInfoWithHandles)
            {
                this.AddWindowInfo(windowInfoWithHandle);
            }
        }

        /**
         * <summary>
         * カレントモニターを取得する
         * </summary> 
         */
        public WindowManager GetCurrentMonitorWindowManager()
        {
            return this.WindowManagers.ElementAt(this.CurrentWindowManagerIndex);
        }

        /**
         * <summary>
         * アクティヴモニターの WindowManager のインデックスを戻す
         * </summary>
         */
        public int GetCurrentWindowManagerIndex()
        {
            return this.CurrentWindowManagerIndex;
        }

        public void SetCurrentWindowManagerIndex(int windowManagerIndex)
        {
            Logger.WriteLine($"MonitorManager.SetCurrentWindowManagerIndex = {windowManagerIndex}");
            this.CurrentWindowManagerIndex = windowManagerIndex;
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
                previousWindowManagerIndex = this.WindowManagers.Count - 1;
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
            if (nextWindowManagerIndex >= this.WindowManagers.Count )
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
            this.SetCurrentWindowManagerIndex(this.WindowManagers.IndexOf(wm));
        }

        /**
         * <summary>
         * this.WindowManagers のうち、指定された monitorNumber のモニター用に管理している
         * WindowManager を戻してこのモニターをアクティヴにする
         * </summary>
         */
        public WindowManager ActivateMonitorNWindowManager(int monitorNumber)
        {
            if (this.MonitorInfos.Count < monitorNumber + 1)
            {
                return null;
            }
            var monitorInfo = this.MonitorInfos.ElementAt(monitorNumber);
            Logger.DebugMonitor($"Get Monitor number={monitorNumber}", monitorInfo);
            this.CurrentWindowManagerIndex = monitorNumber;
            return this.FindWindowManagerByMonitorHandle(monitorInfo.MonitorHandle);
        }

        /**
         * <summary>
         * 指定したモニターハンドルに紐付くWindowManagerを戻す
         * </summary>
         */
        public WindowManager FindWindowManagerByMonitorHandle(IntPtr monitorHandle)
        {
            return this.WindowManagers.Find(
                (WindowManager wm) => { return wm.MonitorHandle == monitorHandle; });
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
            var targetWindowManager = this.FindWindowManagerByMonitorHandle(windowInfoWithHandle.GetMonitorHandle());
            if ( targetWindowManager is null)
            {
                return this.WindowManagers.ElementAt(0);
            }
            Logger.DebugWindowManager("windowManager of MonitorManager.AddWindowInfo", targetWindowManager);
            Logger.DebugWindowInfo("windowInfo of MonitorManager.AddWindowInfo", windowInfoWithHandle);
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
                return this.WindowManagers.ElementAt(0);
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
            var currentMonitorInfo = this.MonitorInfos.Find(
                (MonitorInfoWithHandle mi) => { return mi.MonitorHandle == currentWindowManager.MonitorHandle; });
            //currentMonitorInfo.MonitorInfo.

            Logger.DebugMonitor("Hightlight", currentMonitorInfo);
            MonitorInformationForm monitorInformationForm = new MonitorInformationForm(currentMonitorInfo);
            monitorInformationForm.Show();
        }

        /// <summary>
        /// モニター間移動
        /// </summary>
        /*
        public void MoveMonitor(WindowInfoWithHandle windowInfo)
        {
            IntPtr windowMonitorHandle = windowInfo.MonitorHandle;
            IntPtr nmh = windowInfo.GetMonitor();

            WindowManager owm = new WindowManager(windowMonitorHandle);
            WindowManager nwm = new WindowManager(nmh);

            if ( !WindowManagers.Contains(owm) ||
                !WindowManagers.Contains(nwm))
            {
                // TODO ログ出力？ 例外としてプログラム終了？
                return;
            }

            var oi = WindowManagers.IndexOf(owm);
            var ni = WindowManagers.IndexOf(nwm);

            WindowManagers[oi].Remove(windowInfo);
            WindowManagers[ni].Add(windowInfo);
        }
        */

    }
}
