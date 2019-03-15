using Microservices.Adapters.IDatabase;
using Microservices.Base;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Microservices.Adapters.MemoryDB
{
    public class MemoryAdapter : IKeyvalAdapter
    {
        private static readonly Hashtable datatable = Hashtable.Synchronized(new Hashtable());
        private static readonly Dictionary<string, string> stringdict = new Dictionary<string, string>();
        private static readonly Dictionary<string, DateTime> expiredict = new Dictionary<string, DateTime>();
        private static readonly Dictionary<string, Dictionary<string, string>> hashdict = new Dictionary<string, Dictionary<string, string>>();

        public MemoryAdapter()
        {
        }

        public string Name => "MemoryDBAdapter";

        public Type TargetType => typeof(IKeyvalAdapter);


        public void HashDelete(string key, string field)
        {
            if (hashdict.ContainsKey(key))
                hashdict[key].Remove(field);
        }

        public bool HashExists(string key, string field)
        {
            if (hashdict.ContainsKey(key))
                return hashdict[key].Remove(field);
            else
                return false;
        }

        public string HashGet(string key, string field)
        {
            if (hashdict.ContainsKey(key) && hashdict[key].ContainsKey(field))
                return hashdict[key][field];
            else
                return null;
        }

        public void HashSet(string key, string field, string val)
        {
            if (!hashdict.ContainsKey(key))
                hashdict.Add(key, new Dictionary<string, string>());

            if (hashdict[key].ContainsKey(field))
                hashdict[key][field] = val;
            else
                hashdict[key].Add(field, val);
        }

        public bool KeyDelete(string key)
        {
            expiredict.Remove(key);
            return stringdict.Remove(key);
        }

        public bool KeyExists(string key)
        {
            if (!stringdict.ContainsKey(key))
                return false;

            if (!expiredict.ContainsKey(key))
                return true;

            if (expiredict[key] < DateTime.Now)
                return true;

            KeyDelete(key);
            return false;
        }

        public void KeyExpire(string key, TimeSpan expiry)
        {
            if (stringdict.ContainsKey(key))
            {
                if (expiredict.ContainsKey(key))
                    expiredict[key] = DateTime.Now + expiry;
                else
                    expiredict.Add(key, DateTime.Now + expiry);

                if (expiry == TimeSpan.Zero)
                    KeyDelete(key);
            }
        }

        public void KeyExpire(string key, DateTime expiry)
        {
            if (stringdict.ContainsKey(key))
            {
                if (expiredict.ContainsKey(key))
                    expiredict[key] = expiry;
                else
                    expiredict.Add(key, expiry);

                if (expiry >= DateTime.Now)
                    KeyDelete(key);
            }
        }

        public void KeyExpire(string key)
        {
            KeyDelete(key);
        }

        public string StringGet(string key)
        {
            if (!stringdict.ContainsKey(key))
                return null;

            if (expiredict.ContainsKey(key))
                return stringdict[key];

            if (expiredict[key] <= DateTime.Now)
                return stringdict[key];

            KeyDelete(key);
            return null;
        }

        public void StringSet(string key, string val, TimeSpan? expiry = null)
        {
            if (stringdict.ContainsKey(key))
                stringdict[key] = val;
            else
                stringdict.Add(key, val);

            if (expiry.HasValue)
            {
                if (expiredict.ContainsKey(key))
                    expiredict[key] = DateTime.Now + expiry.Value;
                else
                    expiredict.Add(key, DateTime.Now + expiry.Value);
            }
        }

        public T HashGet<T>(string key, string field) where T : class, ICache, new()
        {
            var val = HashGet(key, field);
            return val == null ? null : JsonConvert.DeserializeObject<T>(val);
        }

        public void HashSet<T>(string key, string field, T val) where T : class, ICache, new()
        {
            HashSet(key, field, JsonConvert.SerializeObject(val));
        }
    }
}
