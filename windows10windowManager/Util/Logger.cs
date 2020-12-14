using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using NLog;
using NLog.Config;
using NLog.Targets;


using windows10windowManager.Window;
using windows10windowManager.Monitor;

namespace windows10windowManager.Util
{
    public class Logger
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /**
         * <summary>
         * Nlogを初期化する
         * </summary>
         */
        public static void Initialize()
        {
            var conf = new LoggingConfiguration();

            var file = new FileTarget("file");
            file.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            file.Layout = "${longdate}[${threadid:padding=2}][${uppercase:${level:padding=-5}}] ${message}${exception:format=Message, Type, ToString:separator=*}";
            file.FileName = "${basedir}/logs/w10wm.log";
            file.ArchiveNumbering = ArchiveNumberingMode.Rolling;
            file.ArchiveFileName = "${basedir}/logs/archives/w10wm.log.{#}";
            file.ArchiveEvery = FileArchivePeriod.Day;
            file.MaxArchiveFiles = 7;
            conf.AddTarget(file);
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));

            LogManager.Configuration = conf;
        }

        /**
         * <summary>
         * ログ出力
         * </summary>
         */
        public static void WriteLine(string message)
        {
            DateTime now = DateTime.Now;
            var line = $"{now} : {message}";

            Debug.WriteLine(line);
            Logger.logger.Debug(message);
        }

        /**
         * <summary>
         * ログ出力。メッセージ、および List<string>を改行で区切ってログ出力する
         * </summary>
         */
        public static void WriteLine(string message, List<string> messages)
        {
            var messagesString = string.Join("\r\n", messages.ToArray());
            Logger.WriteLine($"{message}\r\n{messagesString}");
        }

        /**
         * <summary>
         * Exception用出力
         * </summary>
         */
        public static void Exception(Exception ex)
        {
            Debug.Fail(ex.ToString());
            Logger.logger.Error(ex);
        }

        /**
         * <summary>
         * ログ出力を停止
         * </summary>
         */
        public static void Close()
        {
            NLog.LogManager.Shutdown();
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
