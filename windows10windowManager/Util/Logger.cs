using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using windows10windowManager.Window;

namespace windows10windowManager.Util
{
    class Logger
    {
        /**
         * <summary>
         * ログ出力
         * </summary>
         */
        public static void WriteLine(string message)
        {
            DateTime now = DateTime.Now;
            Debug.WriteLine($"{now} : {message}");
        }

        /**
         * <summary>
         * ウィンドウ情報をデバッグ用に出力する
         * </summary>
         */
        public static void DebugWindowInfo(string message, WindowInfoWithHandle windowInfoWithHandle)
        {
            var windowHandle = windowInfoWithHandle.WindowHandle;
            var windowTitle = windowInfoWithHandle.WindowTitle;
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var m = message + $" : {windowTitle} ( hWnd={windowHandle}, hMonitor={monitorHandle} )";
            Logger.WriteLine(m);
        }

        public static void DebugWindowManager(string message, WindowManager windowManager)
        {
            var monitorHandle = windowManager.MonitorHandle;
            var m = message + $" : hMonitor={monitorHandle}";
            Logger.WriteLine(m);
        }

    }
}
