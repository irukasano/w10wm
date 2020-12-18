using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

using windows10windowManager.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowInfoWithHandleTest
    {
        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [TestMethod]
        public void Test_WindowClose()
        {
            var wiwh = LaunchSampleApplication();
            wiwh.WindowClose();
            Assert.AreEqual(false, this.ExistsWindow(wiwh));
        }

        protected WindowInfoWithHandle LaunchSampleApplication()
        {
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = @"c:\Windows\System32\notepad.exe";
            proc.Start();
            proc.WaitForInputIdle();
            proc.Refresh();

            //Debug.WriteLine(proc.MainWindowHandle);

            var windowInfoWithHandle = new WindowInfoWithHandle(proc.MainWindowHandle);

            return windowInfoWithHandle;
        }

        protected bool ExistsWindow(WindowInfoWithHandle wiwh)
        {
            return IsWindowVisible(wiwh.windowHandle);
        }
    }
}
