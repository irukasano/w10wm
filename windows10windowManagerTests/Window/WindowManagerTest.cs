using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using windows10windowManager.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowManagerTest
    {
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();

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

            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusPrevious()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(2);

            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            var previousWindowInfoWithHandle = wm.MoveCurrentFocusPrevious();
            Assert.AreEqual(1, wm.GetCurrentWindowIndex());
            Assert.IsTrue(previousWindowInfoWithHandle.Equals(windowInfoWithHandle));
            
            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusPreviousOfTop()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(0);

            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            var previousWindowInfoWithHandle = wm.MoveCurrentFocusPrevious();
            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            Assert.IsTrue(previousWindowInfoWithHandle.Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusNext()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(0);

            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            var nextWindowInfoWithHandle = wm.MoveCurrentFocusNext();
            Assert.AreEqual(1, wm.GetCurrentWindowIndex());
            Assert.IsTrue(nextWindowInfoWithHandle.Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusNextOfBottom()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);

            wm.SetCurrentWindowIndex(2);

            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            var nextWindowInfoWithHandle = wm.MoveCurrentFocusNext();
            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            Assert.IsTrue(nextWindowInfoWithHandle.Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusTop()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(2);

            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            var topWindowInfoWithHandle = wm.MoveCurrentFocusTop();
            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            Assert.IsTrue(topWindowInfoWithHandle.Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_MoveCurrentFocusBottom()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);

            wm.SetCurrentWindowIndex(0);

            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            var bottomWindowInfoWithHandle = wm.MoveCurrentFocusBottom();
            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            Assert.IsTrue(bottomWindowInfoWithHandle.Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowPrevious()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(1);

            Assert.AreEqual(1, wm.GetCurrentWindowIndex());
            wm.SetWindowPrevious();
            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowPreviousOfTop()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(0);

            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            wm.SetWindowPrevious();
            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowNext()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(1);

            Assert.AreEqual(1, wm.GetCurrentWindowIndex());
            wm.SetWindowNext();
            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowNextOfBottom()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);

            wm.SetCurrentWindowIndex(2);

            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            wm.SetWindowNext();
            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowTop()
        {
            var wm = new WindowManager(IntPtr.Zero);

            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());
            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);

            wm.SetCurrentWindowIndex(2);

            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            wm.SetWindowTop();
            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }

        [TestMethod]
        public void Test_SetWindowBottom()
        {
            var wm = new WindowManager(IntPtr.Zero);

            var windowInfoWithHandle = this.LaunchSampleApplication();
            wm.Add(windowInfoWithHandle);
            wm.Add(this.LaunchSampleApplication());
            wm.Add(this.LaunchSampleApplication());

            wm.SetCurrentWindowIndex(0);

            Assert.AreEqual(0, wm.GetCurrentWindowIndex());
            wm.SetWindowBottom();
            Assert.AreEqual(2, wm.GetCurrentWindowIndex());
            Assert.IsTrue(wm.GetCurrentWindow().Equals(windowInfoWithHandle));

            this.CloseSampleApplications(wm);
        }



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
