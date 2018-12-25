using Microservices.Adapters.IDatabase;
using Microservices.Base;
using System;

namespace Microservices.Adapters.Redis
{
    public interface IKeyvalAdapter : IAdapter
    {
        string StringGet(string key);

        void StringSet(string key, string val);

        void StringSet(string key, string val, TimeSpan expiry);

        string HashGet(string field, string key);

        void HashSet(string field, string key, string val);

        T HashGet<T>(string field, string key) where T : class, ICache, new();

        void HashSet<T>(string field, string key, T val) where T : class, ICache, new();

        bool HashExists(string field, string key);

        void Expire(string key, TimeSpan expiry);

        void Expire(string key, DateTime expiry);

        bool Exists(string key);
    }
}
