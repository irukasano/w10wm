using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


using windows10windowManagerUtil;
using windows10windowManagerWindowHook;
using windows10windowManager.Window;
using windows10windowManager.Monitor;

namespace windows10windowManager.Util
{
    public class Logger : windows10windowManagerUtil.Util.Logger
    {
        public static void DebugWindowManager(string message, WindowManager windowManager)
        {
            var monitorHandle = windowManager.monitorHandle;
            var m = message + $" : hMonitor={monitorHandle}";
            Logger.WriteLine(m);
        }

    }
}
