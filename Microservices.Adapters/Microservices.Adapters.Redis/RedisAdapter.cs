using Microservices.Adapters.IDatabase;
using Microservices.Common;
using Newtonsoft.Json;
using System;

namespace Microservices.Adapters.Redis
{
    public class RedisAdapter : IKeyvalAdapter
    {
        private RedisClient client;
        private StackExchange.Redis.IDatabase database;

        public string Name { get => "RedisAdapter"; }
        public Type TargetType { get => typeof(IKeyvalAdapter); }
        public RedisAdapter()
        {
            string connstr = Config.Root["Redis"];
            if (string.IsNullOrEmpty(connstr))
                throw new Exception("Config Not Found: [Redis]");
            this.client = new RedisClient(connstr, Config.Root["RedisDatabase"]);
            this.database = this.client.GetDatabase();
        }

        public string StringGet(string key)
        {
            try
            {
                return database.StringGet(key);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StringSet(string key, string val)
        {
            try
            {
                database.StringSet(key, val);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StringSet(string key, string val, TimeSpan expiry)
        {
            try
            {
                database.StringSet(key, val, expiry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T HashGet<T>(string field, string key) where T : class, ICache, new()
        {
            try
            {
                var json = database.HashGet(key, field);
                if (!string.IsNullOrEmpty(json))
                    return JsonConvert.DeserializeObject<T>(json);
                else
                    return new T();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void HashSet<T>(string field, string key, T val) where T : class, ICache, new()
        {
            try
            {
                database.HashSet(key, field, JsonConvert.SerializeObject(val));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string HashGet(string field, string key)
        {
            try
            {
                return database.HashGet(key, field);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void HashSet(string field, string key, string val)
        {
            try
            {
                database.HashSet(key, field, val);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool HashExists(string field, string key)
        {
            try
            {
                return database.HashExists(key, field);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Expire(string key, TimeSpan expiry)
        {
            try
            {
                database.KeyExpire(key, expiry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Expire(string key, DateTime expiry)
        {
            try
            {
                database.KeyExpire(key, expiry);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Exists(string key)
        {
            try
            {
                return database.KeyExists(key);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
