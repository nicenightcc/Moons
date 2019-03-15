using Microservices.Common;
using StackExchange.Redis;
using System;

namespace Microservices.Adapters.Redis
{
    public class RedisClient : IDisposable
    {
        private string connectionString;
        private ConnectionMultiplexer connection;

        public RedisClient(string connectionString)
        {
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
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
