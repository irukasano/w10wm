using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using windows10windowManager.Window;
using windows10windowManagerUtil.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowTilerDividerBugnTest
    {
        [TestMethod]
        public void Test_GetWindowRectOf_NoWindow()
        {
            WindowTilerBugn windowTiler = new WindowTilerBugn();

            windowTiler.CalcuratePosition(0, 0, 1000, 0, 2000);
            Assert.IsTrue(windowTiler.defaultWindowRect.Equals(windowTiler.GetWindowRectOf(0)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_CountOfRightStackWindowIsZero()
        {
            WindowTilerBugn windowTiler = new WindowTilerBugn();
            windowTiler.percentOfLeftColumn = 0.7;

            windowTiler.CalcuratePosition(1, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 1400).Equals(windowTiler.GetWindowRectOf(0)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_CountOfRightStackWindowIsOne()
        {
            WindowTilerBugn windowTiler = new WindowTilerBugn();
            windowTiler.percentOfLeftColumn = 0.7;
            windowTiler.maxCountOfWindowOfRightColumn = 6;

            windowTiler.CalcuratePosition(2, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 1400).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(0, 1000, 1400, 2000).Equals(windowTiler.GetWindowRectOf(1)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_CountOfRightStackWindowIsThree()
        {
            WindowTilerBugn windowTiler = new WindowTilerBugn();
            windowTiler.percentOfLeftColumn = 0.7;
            windowTiler.maxCountOfWindowOfRightColumn = 6;

            windowTiler.CalcuratePosition(5, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 1400).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(0, 250, 1400, 2000).Equals(windowTiler.GetWindowRectOf(1)));
            Assert.IsTrue(new WindowRect(250, 500, 1400, 2000).Equals(windowTiler.GetWindowRectOf(2)));
            Assert.IsTrue(new WindowRect(500, 750, 1400, 2000).Equals(windowTiler.GetWindowRectOf(3)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2000).Equals(windowTiler.GetWindowRectOf(4)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_CountOfRightStackWindowIsOverMax()
        {
            WindowTilerBugn windowTiler = new WindowTilerBugn();
            windowTiler.percentOfLeftColumn = 0.7;
            windowTiler.maxCountOfWindowOfRightColumn = 4;

            windowTiler.CalcuratePosition(8, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 1400).Equals(windowTiler.GetWindowRectOf(0)));
            Assert.IsTrue(new WindowRect(0, 250, 1400, 2000).Equals(windowTiler.GetWindowRectOf(1)));
            Assert.IsTrue(new WindowRect(250, 500, 1400, 2000).Equals(windowTiler.GetWindowRectOf(2)));
            Assert.IsTrue(new WindowRect(500, 750, 1400, 2000).Equals(windowTiler.GetWindowRectOf(3)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2000).Equals(windowTiler.GetWindowRectOf(4)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2000).Equals(windowTiler.GetWindowRectOf(5)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2000).Equals(windowTiler.GetWindowRectOf(7)));
        }
    }
}

