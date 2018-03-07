using classlib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mstest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("Test", (new Class1()).Value);
        }
    }
}
