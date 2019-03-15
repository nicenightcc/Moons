using Microservices.Base;
using System;

namespace Microservices.Adapters.IDatabase
{
    public interface IKeyvalAdapter : IAdapter
    {
        string StringGet(string key);

        void StringSet(string key, string val, TimeSpan? expiry = null);

        void KeyExpire(string key, TimeSpan expiry);

        void KeyExpire(string key, DateTime expiry);

        void KeyExpire(string key);

        bool KeyExists(string key);

        bool KeyDelete(string key);

        string HashGet(string key, string field);

        void HashSet(string key, string field, string val);

        T HashGet<T>(string key, string field) where T : class, ICache, new();

        void HashSet<T>(string key, string field, T val) where T : class, ICache, new();

        void HashDelete(string key, string field);

        bool HashExists(string key, string field);
    }
}
