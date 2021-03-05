using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using windows10windowManager.Window;
using windows10windowManagerUtil.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowTilerMdiTest
    {
        [TestMethod]
        public void Test_GetWindowRectOf_NoWindow()
        {
            WindowTilerMdi windowTiler = new WindowTilerMdi();

            windowTiler.CalcuratePosition(0, 0, 1000, 0, 2000);
            Assert.IsTrue(windowTiler.defaultWindowRect.Equals(windowTiler.GetWindowRectOf(0)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_MultiWindowIsInMonitorRect()
        {
            WindowTilerMdi windowTiler = new WindowTilerMdi();
            //windowTiler.percentOfMonitorHeight = 0.1;
            //windowTiler.percentOfMonitorWidth = 0.2;
            windowTiler.marginTopToMonitor = 10;
            windowTiler.marginBottomToMonitor = 20;
            windowTiler.marginLeftToMonitor = 30;
            windowTiler.marginRightToMonitor = 40;
            windowTiler.shiftHeightToNextWindow = 50;
            windowTiler.shiftWidthToNextWindow = 60;

            windowTiler.CalcuratePosition(3, 0, 1000, 0, 2000);
            /*
             * windowHeight = 100;
             * windowWidth = 400;
             */
            Assert.IsTrue(new WindowRect(10, 100 + 10, 30, 30 + 400).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(10 + 50, 100 + 10 + 50, 30 + 60, 30 + 60 + 400).Equals(windowTiler.GetWindowRectOf(1)));
            Assert.IsTrue(new WindowRect(10 + 50 * 2, 100 + 10 + 50 * 2, 30 + 60 * 2, 30 + 60 * 2 + 400).Equals(windowTiler.GetWindowRectOf(2)));
        }


        [TestMethod]
        public void Test_GetWindowRectOf_MultiWindowOverBottom()
        {
            WindowTilerMdi windowTiler = new WindowTilerMdi();
            //windowTiler.percentOfMonitorHeight = 0.8;
            //windowTiler.percentOfMonitorWidth = 0.2;
            windowTiler.marginTopToMonitor = 10;
            windowTiler.marginBottomToMonitor = 10;
            windowTiler.marginLeftToMonitor = 10;
            windowTiler.marginRightToMonitor = 10;
            windowTiler.shiftHeightToNextWindow = 100;
            windowTiler.shiftWidthToNextWindow = 50;

            windowTiler.CalcuratePosition(3, 0, 1000, 0, 2000);
            /*
             * windowHeight = 800;
             * windowWidth = 400;
             */
            Assert.IsTrue(new WindowRect(10, 810, 10, 410).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(110, 910, 60, 460).Equals(windowTiler.GetWindowRectOf(1)));
            Assert.IsTrue(new WindowRect(10, 810, 60, 460).Equals(windowTiler.GetWindowRectOf(2)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_MultiWindowOverRight()
        {
            WindowTilerMdi windowTiler = new WindowTilerMdi();
            //windowTiler.percentOfMonitorHeight = 0.8;
            //windowTiler.percentOfMonitorWidth = 0.6;
            windowTiler.marginTopToMonitor = 10;
            windowTiler.marginBottomToMonitor = 10;
            windowTiler.marginLeftToMonitor = 10;
            windowTiler.marginRightToMonitor = 10;
            windowTiler.shiftHeightToNextWindow = 100;
            windowTiler.shiftWidthToNextWindow = 400;

            windowTiler.CalcuratePosition(4, 0, 1000, 0, 2000);
            /*
             * windowHeight = 800;
             * windowWidth = 1200;
             */
            Assert.IsTrue(new WindowRect(10, 810, 10, 1210).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(110, 910, 410, 1610).Equals(windowTiler.GetWindowRectOf(1)));
            Assert.IsTrue(new WindowRect(10, 810, 410, 1610).Equals(windowTiler.GetWindowRectOf(2)));
            Assert.IsTrue(new WindowRect(10, 810, 10, 1210).Equals(windowTiler.GetWindowRectOf(3)));
        }
    }
}


