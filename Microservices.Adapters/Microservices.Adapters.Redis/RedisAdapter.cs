using Microservices.Adapters.IDatabase;
using Microservices.Base;
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
            string redisdb = Config.Root["RedisDB"];
            var db = redisdb == null ? 0 : int.Parse(redisdb);
            this.client = new RedisClient(connstr);
            this.database = this.client.GetDatabase(db);
        }

        public string StringGet(string key)
        {
            try
            {
                return database.StringGet(key);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
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
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void StringSet(string key, string val, TimeSpan? expiry = null)
        {
            try
            {
                database.StringSet(key, val, expiry);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T HashGet<T>(string key, string field) where T : class, ICache, new()
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
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void HashSet<T>(string key, string field, T val) where T : class, ICache, new()
        {
            try
            {
                database.HashSet(key, field, JsonConvert.SerializeObject(val));
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public string HashGet(string key, string field)
        {
            try
            {
                return database.HashGet(key, field);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void HashSet(string key, string field, string val)
        {
            try
            {
                database.HashSet(key, field, val);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public bool HashExists(string key, string field)
        {
            try
            {
                return database.HashExists(key, field);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void HashDelete(string key, string field)
        {
            try
            {
                database.HashDelete(key, field);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void KeyExpire(string key, TimeSpan expiry)
        {
            try
            {
                database.KeyExpire(key, expiry);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void KeyExpire(string key, DateTime expiry)
        {
            try
            {
                database.KeyExpire(key, expiry);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void KeyExpire(string key)
        {
            try
            {
                database.KeyExpire(key, TimeSpan.Zero);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public bool KeyExists(string key)
        {
            try
            {
                return database.KeyExists(key);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public bool KeyDelete(string key)
        {
            try
            {
                return database.KeyDelete(key);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}
