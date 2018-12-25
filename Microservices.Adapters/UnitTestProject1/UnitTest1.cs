using ClassLibrary1;
using Microservices.Adapters.IDatabase;
using Microservices.Adapters.EF.SQLite;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            Config.Root.Load(Path.Combine(Environment.CurrentDirectory, "config.json"));
            IoCFac.Instance.Load(Path.Combine(Environment.CurrentDirectory, "lib"));
            var t = IoCFac.Instance.GetByBase<IEntity>();
        }
        [TestMethod]
        public void TestMethod1()
        {
            var sqliteAdapter = new SQLiteAdapter();
            sqliteAdapter.Add(new User { Name = "aaa" });
        }
    }
}
