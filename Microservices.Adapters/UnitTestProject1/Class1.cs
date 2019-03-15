using Microservices.Adapters.IDatabase;
using System.Linq;

namespace UnitTestProject1
{
    public class Class1
    {
        public void Run(ISqlAdapter sqlAdapter)
        {
            var a = sqlAdapter.Query<UserCenter.DataModel.DB_User>();
            var aa = a.ToList();
        }
    }
}
