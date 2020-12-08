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
        public void Test_A()
        {
            WindowTilerDivider windowTiler = new WindowTilerDivider();
            windowTiler.CalcuratePosition(6, 0, 0, 0, 0);
            Assert.IsTrue(new WindowRect(0, 0, 0, 0).Equals(windowTiler.GetWindowRectOf(0)));
        }

    }
}
