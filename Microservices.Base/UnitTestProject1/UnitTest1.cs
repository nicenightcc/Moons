using Microservices.Adapters;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TestService;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Config.Root.Load("config.json");
            var conf = Config.Root;
        }
        [TestMethod]
        public void TestMethod2()
        {
            Log.SetName("test");
            Log.Logger.Info("helo{0}{1}{2}", "a", "b", "c");
        }
        [TestMethod]
        public void TestMethod3()
        {
            IoCFac.Instance.LoadAssembly("UnitTestProject1");
            var types = IoCFac.Instance.GetAll();
            var adapter = IoCFac.Instance.GetClass<ITestAdapter>();
        }
        [TestMethod]
        public void TestMethod4()
        {
            AdapterFac.Instance.Register(typeof(TestAdapter));
            var adapters = AdapterFac.Instance.GetAll();
            var testAdapter = AdapterFac.Instance.GetAdapter<ITestAdapter>();
            var result = testAdapter.Test();
        }
        [TestMethod]
        public void TestMethod5()
        {
            var a = Directory.Exists("logs");
            var b = File.Exists("log4net.config");
        }
    }
}
