using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using windows10windowManager.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowManagerTest
    {
        [TestMethod]
        public void Test_AddAndCountRight()
        {
            var wm = new WindowManager(IntPtr.Zero);
            Assert.AreEqual(0, wm.WindowCount());


            wm.Add(this.LaunchSampleApplication());
            Assert.AreEqual(1, wm.WindowCount());

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_RemoveAndCountRight()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Remove(windowInfoWithHandle);

            Assert.AreEqual(0, wm.WindowCount());

            wm.Add(windowInfoWithHandle);
            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_GetCurrentWindow()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);

            wm.Add(this.LaunchSampleApplication());

            Assert.AreEqual(true, wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
       


        protected WindowInfoWithHandle LaunchSampleApplication(
            string FileName = @"c:\Windows\System32\notepad.exe")
        {
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = FileName;
            proc.Start();
            proc.WaitForInputIdle();
            proc.Refresh();

            Debug.WriteLine(FileName + " = " + proc.MainWindowHandle);

            var windowInfoWithHandle = new WindowInfoWithHandle(proc.MainWindowHandle);

            return windowInfoWithHandle;
        }

        protected void CloseSampleApplications(WindowManager wm)
        {
            foreach (WindowInfoWithHandle windowInfoWithHandle in wm.GetWindows())
            {
                windowInfoWithHandle.WindowClose();
            }
        }
    }
}
