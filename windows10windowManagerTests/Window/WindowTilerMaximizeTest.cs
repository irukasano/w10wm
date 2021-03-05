using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using windows10windowManager.Window;
using windows10windowManagerUtil.Window;

namespace windows10windowManagerTests.Window
{
    [TestClass]
    public class WindowTilerMaximizeTest
    {
        [TestMethod]
        public void Test_GetWindowRectOf_NoWindow()
        {
            WindowTilerMaximize windowTiler = new WindowTilerMaximize();
            windowTiler.CalcuratePosition(0, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 2000).Equals(windowTiler.GetWindowRectOf(0)));

        }

        [TestMethod]
        public void Test_GetWindowRectOf_AnyWindowWillMaximize()
        {
            WindowTilerMaximize windowTiler = new WindowTilerMaximize();
            windowTiler.CalcuratePosition(6, 0, 1000, 0, 2000);
            Assert.IsTrue(new WindowRect(0, 1000, 0, 2000).Equals(windowTiler.GetWindowRectOf(3)));

        }
    }
}

