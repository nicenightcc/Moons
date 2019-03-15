//using Microservices.Adapters.IDatabase;
using Microservices.Adapters.EF.MySql;
using Microservices.Adapters.WebApi;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserCenter.DataModel;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        //ISqlAdapter sqlAdapter;
        //IKeyvalAdapter keyvalAdapter;
        public UnitTest1()
        {
            Config.Root.Load("config.json");
            //IoCFac.Instance.LoadAssembly("UserCenter.WebApi");

            //sqlAdapter = new MySqlAdapter();
            //keyvalAdapter = new RedisAdapter();
        }
        //[TestMethod]
        //public void TestMethod1()
        //{
        //    var api = new WebApiAdapter();
        //    //var sqliteAdapter = new SQLiteAdapter();
        //    //sqliteAdapter.Add(new User { Name = "aaa" });
        //}
        //[TestMethod]
        //public void TestMethod2()
        //{
        //    //keyvalAdapter.StringSet("a", "1");
        //    //var a = keyvalAdapter.StringGet("a");
        //}
        //[TestMethod]
        //public void TestMethod3()
        //{
        //    var list = new List<Task>();
        //    var result = new List<object>();
        //    for (var i = 0; i < 20; i++)
        //    {
        //        list.Add(
        //        Task.Run(() =>
        //        {
        //            //var a = sqlAdapter.Find<User>(en => en.ID == 1);
        //            //result.Add(a);
        //        }));
        //    }
        //    Task.WaitAll(list.ToArray());
        //}
        //[TestMethod]
        //public void TestMethod4()
        //{
        //    WebApiAdapter apiAdapter = new WebApiAdapter();
        //    //var a = apiAdapter.Call(new UserCenter.WebApi.GetUserRequest { Token = "5f0e048e-0d5d-42ff-9435-8b3beb293073" });
        //}
        //[TestMethod]
        //public void TestMethod5()
        //{
        //    //Config.Root.Load("config.json");
        //    //new Microservices.Builder.ServiceBuilder()
        //    //.LoadAssembly("UserCenter.WebApi")
        //    //.UseAdapter<WebApiAdapter>()
        //    //.UseWebApi()
        //    //.Run();
        //    //WebApiAdapter apiAdapter = new WebApiAdapter();
        //    //var a = apiAdapter.Call(new TestService.TestRequest { });
        //}
        //[TestMethod]
        //public void TestMethod6()
        //{
        //    IoCFac.Instance.LoadAssembly("UnitTestProject1");


        //    var sqlAdapter = new MySqlAdapter();

        //    var a = sqlAdapter.Query<UserCenter.DataModel.DB_User>();
        //    //var b = a.Where(en => en.ID == 1);
        //    var b = a.Where(en => en.DataStatus == 1);
        //    var b2 = b.Where(en => en.ID == 1);
        //    var a1 = b.ToList();
        //    var a2 = b2.ToList();
        //    var a3 = b.Count();
        //    var a4 = b.Select(en => en.Password).ToList();

        //    //var a1 = sqlAdapter.Query<UserCenter.DataModel.DB_User>(a.Expression);
        //    //var aa1 = a1.Where(en => en.DataStatus == 2).ToList();

        //    var bs = sqlAdapter.Query<UserCenter.DataModel.DB_User>();
        //    //var b = sqlAdapter.Query<UserCenter.DataModel.DB_User>();
        //    //var bb = ((Queryable<UserCenter.DataModel.DB_User>)b).ToList();
        //}

        //public void test()
        //{
        //}
        //[TestMethod]
        //public void TestMethod7()
        //{
        //    IoCFac.Instance.LoadAssembly("UnitTestProject1");
        //    Task.WaitAll(
        //    Task.Run(() =>
        //    {
        //        var sqlAdapter = new MySqlAdapter();

        //        var a = sqlAdapter.Query<UserCenter.DataModel.DB_User>().Where(en => en.DataStatus == 1);
        //        var h1 = a.ToList();
        //    }),
        //    Task.Run(() =>
        //    {
        //        var sqlAdapter2 = new MySqlAdapter();

        //        var aa = sqlAdapter2.Query<UserCenter.DataModel.DB_User>().Where(en => en.DataStatus == 1);
        //        var ha1 = aa.ToList();
        //    }));
        //}
        [TestMethod]
        public void TestMethod8()
        {
            //IQueryable<UserCenter.DataModel.DB_User> query = new MarkQueryable<UserCenter.DataModel.DB_User>();
            //query = query.Where(en => en.DataStatus == 1);
            //query = query.Where(en => en.ID == 1);
            //var list = query.ToList();

            IoCFac.Instance.LoadAssembly("UnitTestProject1");
            var sqlAdapter = new MySqlAdapter();
            var groupid = sqlAdapter.Query<DB_UserGroup>(en => en.UserID == 1 && en.DataStatus == (int)DataStatus.Normal).Select(en => en.GroupID).ToList();
            var groups = sqlAdapter.Query<DB_Group>(en => groupid.Contains(en.ID) && en.DataStatus == (int)DataStatus.Normal).ToList();

        }
    }
}
