using StackExchange.Redis;
using System;

namespace Microservices.Adapters.Redis
{
    public class RedisClient : IDisposable
    {
        private string connectionString;
        private int database = 0;
        private ConnectionMultiplexer connection;

        public RedisClient(string connectionString, string database)
        {
            if (!int.TryParse(database, out this.database))
                this.database = 0;
            this.connectionString = connectionString;
            this.connection = ConnectionMultiplexer.Connect(this.connectionString);
        }

        public StackExchange.Redis.IDatabase GetDatabase(int database = 0)
        {
            try
            {
                return connection.GetDatabase(database);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
