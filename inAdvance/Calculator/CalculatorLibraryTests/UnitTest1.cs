using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorLibrary;

namespace CalculatorLibraryTests
{
    [TestClass]
    public class UnitTest1
    {
        private Calculator calculator;

        public UnitTest1()
        {
            calculator = new Calculator();
        }

        [TestMethod]
        public void TestAdd()
        {
            var r = calculator.DoOperation(1, 2, "a");
            Assert.AreEqual(3, r);
        }

        [TestMethod]
        public void TestSub()
        {
            var r = calculator.DoOperation(2, 1, "s");
            Assert.AreEqual(1, r);
        }
    }
}
