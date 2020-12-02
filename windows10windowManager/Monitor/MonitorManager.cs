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

        public bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var mi = new MONITORINFO();
            mi.size = (uint)Marshal.SizeOf(mi);
            GetMonitorInfo(hMonitor, ref mi);

            // Add to monitor info
            MonitorInfos.Add(new MonitorInfoWithHandle(hMonitor, mi));
            return true;
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
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, this.MonitorEnum, IntPtr.Zero);

            return this.MonitorInfos;
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
         * カレントウィンドウの WindowInfos 内のインデックスを取得する
         * </summary>
         */
        public int GetCurrentWindowManagerIndex()
        {
            return this.CurrentWindowManagerIndex;
        }

        public void SetCurrentWindowManagerIndex(int windowManagerIndex)
        {
            this.CurrentWindowManagerIndex = windowManagerIndex;
        }

        /**
         * <summary>
         * WindowInfoWithHandle をモニターのWindowManagerに追加する
         * </summary>
         */
        public WindowManager AddWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            IntPtr mh = windowInfoWithHandle.GetMonitor();
            WindowManager kwm = new WindowManager(mh);

            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            if (this.WindowManagers.Contains(kwm))
            {
                var i = WindowManagers.IndexOf(kwm);
                this.WindowManagers[i].Add(windowInfoWithHandle);
                return this.WindowManagers[i];
            }

            return this.WindowManagers[0];
        }

        /**
         * <summary>
         * WindowInfoWithHandle をモニターの WindowManager から削除する
         * </summary>
         */
        public WindowManager RemoveWindowInfo(WindowInfoWithHandle windowInfoWithHandle)
        {
            IntPtr mh = windowInfoWithHandle.GetMonitor();
            WindowManager kwm = new WindowManager(mh);

            // このウィンドウのモニターのウィンドウを管理する WindowManager が存在すれば
            // これに追加して管理させる
            if (this.WindowManagers.Contains(kwm))
            {
                var i = WindowManagers.IndexOf(kwm);
                this.WindowManagers[i].Remove(windowInfoWithHandle);
                return this.WindowManagers[i];
            }

            return this.WindowManagers[0];
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
