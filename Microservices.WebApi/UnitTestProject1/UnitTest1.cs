using Microservices.Builder;
using Microservices.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Config.Root.Load("config.json");
            string urls = Config.Root["WebApi"]["urls"];
            new Microservices.Builder.ServiceBuilder().LoadAssembly("TestService").UseWebApi().Run();
        }
        [TestMethod]
        public void TestMethod2()
        {
            var b = Type.GetType("Microservices.WebApi.WebApiAdapter, Microservices.WebApi");
            var t = typeof(Microservices.WebApi.WebApiAdapter);
            var a = t.AssemblyQualifiedName;
        }
    }
}
