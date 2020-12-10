using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using windows10windowManager.Window;
using windows10windowManager.Monitor;

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
            var windowHandle = windowInfoWithHandle.windowHandle;
            var windowTitle = windowInfoWithHandle.windowTitle;
            var monitorHandle = windowInfoWithHandle.GetMonitorHandle();
            var m = message + $" : {windowTitle} ( hWnd={windowHandle}, hMonitor={monitorHandle} )";
            Logger.WriteLine(m);
        }

        public static void DebugWindowManager(string message, WindowManager windowManager)
        {
            var monitorHandle = windowManager.monitorHandle;
            var m = message + $" : hMonitor={monitorHandle}";
            Logger.WriteLine(m);
        }

        public static void DebugMonitor(string message, MonitorInfoWithHandle monitorInfoWithHandle)
        {
            var deviceName = new string(monitorInfoWithHandle.monitorInfo.szDevice).TrimEnd('\0');
            var monitorHandle = monitorInfoWithHandle.monitorHandle;
            var monitorRect = monitorInfoWithHandle.monitorRect;
            var top = monitorRect.top;
            var bottom = monitorRect.bottom;
            var left = monitorRect.left;
            var right = monitorRect.right;
            var m = message + $" : {deviceName} (top={top},bottom={bottom},left={left},right={right}) ( hMonitor={monitorHandle} )";
            Logger.WriteLine(m);
        }

    }
}
