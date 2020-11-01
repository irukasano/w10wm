using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using windows10windowManager.Window;

namespace windows10windowManagerTests
{
    [TestClass]
    public class WindowManagerTest
    {
        [TestMethod]
        public void TestAdd()
        {
            var wm = new WindowManager(IntPtr.Zero);
            Assert.AreEqual(0, wm.WindowCount());

        }
    }
}
