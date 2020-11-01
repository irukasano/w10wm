using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

//using Window;
using windows10windowManager.Window;

namespace windows10windowManager.Monitor
{

    class MonitorManager
    {

        /// <summary>
        /// Monitor Enum Delegate
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor.</param>
        /// <param name="hdcMonitor">A handle to a device context.</param>
        /// <param name="lprcMonitor">A pointer to a RECT structure.</param>
        /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the enumeration function.</param>
        /// <returns></returns>
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor,
            ref RECT lprcMonitor, IntPtr dwData);

        /// <summary>
        /// Enumerates through the display monitors.
        /// </summary>
        /// <param name="hdc">A handle to a display device context that defines the visible region of interest.</param>
        /// <param name="lprcClip">A pointer to a RECT structure that specifies a clipping rectangle.</param>
        /// <param name="lpfnEnum">A pointer to a MonitorEnumProc application-defined callback function.</param>
        /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the MonitorEnumProc function.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
            MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        /// <summary>
        /// Gets the monitor information.
        /// </summary>
        /// <param name="hmon">A handle to the display monitor of interest.</param>
        /// <param name="mi">A pointer to a MONITORINFO instance created by this method.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hmon, ref MONITORINFO mi);

        public List<MonitorInfoWithHandle> MonitorInfos { get; set; }

        public List<WindowManager> WindowManagers { get; set; }

        public MonitorManager(TraceWindow traceWindow)
        {
            FindMonitors();

            // 渡された WindowHandles を渡し、モニターごとにWindowManagerで管理する
            ManageWindowByMonitors(traceWindow.WindowHandles);

        }

        /// <summary>
        /// Monitor Enum Delegate
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor.</param>
        /// <param name="hdcMonitor">A handle to a device context.</param>
        /// <param name="lprcMonitor">A pointer to a RECT structure.</param>
        /// <param name="dwData">Application-defined data that EnumDisplayMonitors passes directly to the enumeration function.</param>
        /// <returns></returns>
        public bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var mi = new MONITORINFO();
            mi.size = (uint)Marshal.SizeOf(mi);
            GetMonitorInfo(hMonitor, ref mi);

            // Add to monitor info
            MonitorInfos.Add(new MonitorInfoWithHandle(hMonitor, mi));
            return true;
        }

        /// <summary>
        /// Gets the monitors.
        /// </summary>
        /// <returns></returns>
        public List<MonitorInfoWithHandle> FindMonitors()
        {
            // New List
            MonitorInfos = new List<MonitorInfoWithHandle>();

            // Enumerate monitors
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, IntPtr.Zero);

            return MonitorInfos;
        }

        /// <summary>
        /// WindowHandleを各モニターごとに管理する
        /// </summary>
        /// <param name="windowHandles"></param>
        public void ManageWindowByMonitors(List<WindowInfoWithHandle> windowHandles)
        {
            WindowManagers.Clear();

            foreach (var monitorInfo in MonitorInfos)
            {
                WindowManagers.Add(new WindowManager(monitorInfo.MonitorHandle));
            }

            foreach (var windowHandle in windowHandles)
            {
                IntPtr mh = windowHandle.GetMonitor();
                WindowManager kwm = new WindowManager(mh);

                if (WindowManagers.Contains(kwm))
                {
                    var i = WindowManagers.IndexOf(kwm);
                    WindowManagers[i].Add(windowHandle);
                }

            }
        }

        /// <summary>
        /// モニター間移動
        /// </summary>
        public void MoveMonitor(WindowInfoWithHandle windowInfo)
        {
            IntPtr omh = windowInfo.MonitorHandle;
            IntPtr nmh = windowInfo.GetMonitor();

            WindowManager owm = new WindowManager(omh);
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

    }
}
