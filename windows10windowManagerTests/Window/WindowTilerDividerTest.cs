using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using windows10windowManager.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowTilerDividerTest
    {
        [TestMethod]
        public void Test_GetWindowRectOf_NoWindow()
        {
            WindowTilerDivider windowTiler = new WindowTilerDivider();
            windowTiler.columnCount = 3;
            windowTiler.rowCount = 4;
            windowTiler.CalcuratePosition(0, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 0, 0, 0).Equals(windowTiler.GetWindowRectOf(0)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_TopOfMultiWindow()
        {
            WindowTilerDivider windowTiler = new WindowTilerDivider();
            windowTiler.columnCount = 3;
            windowTiler.rowCount = 4;
            windowTiler.CalcuratePosition(12, 0, 1000, 0, 2100);
            Assert.IsTrue(new WindowRect(0, 250, 0, 700).Equals(windowTiler.GetWindowRectOf(0)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_MinusOfMultiWindow()
        {
            WindowTilerDivider windowTiler = new WindowTilerDivider();
            windowTiler.columnCount = 3;
            windowTiler.rowCount = 4;
            windowTiler.CalcuratePosition(12, 0, 1000, 0, 2100);
            Assert.IsTrue(new WindowRect(0, 250, 0, 700).Equals(windowTiler.GetWindowRectOf(-1)));
        }

        [TestMethod]
        public void Test_GetWindowRectOf_OverCountOfMultiWindow()
        {
            WindowTilerDivider windowTiler = new WindowTilerDivider();
            windowTiler.columnCount = 3;
            windowTiler.rowCount = 4;
            windowTiler.CalcuratePosition(12, 0, 1000, 0, 2100);
            Assert.IsTrue(new WindowRect(500, 750, 1400, 2100).Equals(windowTiler.GetWindowRectOf(10)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2100).Equals(windowTiler.GetWindowRectOf(11)));
            Assert.IsTrue(new WindowRect(750, 1000, 1400, 2100).Equals(windowTiler.GetWindowRectOf(12)));
        }
    }
}

