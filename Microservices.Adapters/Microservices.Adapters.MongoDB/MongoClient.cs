using global::MongoDB.Driver;
using Microservices.Common;
using System;

namespace Microservices.Adapters.MongoDB
{
    public class MongoClient : IDisposable
    {
        private string connectionString;
        private string database;
        private global::MongoDB.Driver.MongoClient client;

        public MongoClient(string connectionString)
        {
            var slantcount = connectionString.Split('/').Length;
            if (slantcount > 3)
            {
                this.database = connectionString.Substring(connectionString.LastIndexOf('/') + 1);
                this.connectionString = connectionString.Substring(0, connectionString.LastIndexOf('/'));
            }
            else
            {
                this.connectionString = connectionString;
            }

            this.client = new global::MongoDB.Driver.MongoClient(this.connectionString);
        }

        public IMongoDatabase GetDatabase(string database = "")
        {
            if (string.IsNullOrEmpty(database))
            {
                if (string.IsNullOrEmpty(this.database))
                    throw new Exception("MongoDB : 未指定数据库");
                else
                    database = this.database;
            }
            try
            {
                return client.GetDatabase(database);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void Dispose()
        {

        }
    }
}
