using global::MongoDB.Driver;
using global::MongoDB.Driver.Linq;
using Microservices.Adapters.IDatabase;
using Microservices.Base;
using Microservices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservices.Adapters.MongoDB
{
    public class MongoAdapter : IJsonAdapter
    {
        private MongoClient client;
        private IMongoDatabase database;

        public string Name { get => "MongoAdapter"; }
        public Type TargetType { get => typeof(IJsonAdapter); }

        public MongoAdapter()
        {
            string connstr = Config.Root["Mongo"];
            if (string.IsNullOrEmpty(connstr))
                throw new Exception("Config Not Found: [Mongo]");
            this.client = new MongoClient(connstr);
            this.database = this.client.GetDatabase();
        }

        public void Add<T>(T entity) where T : class, ICache, new()
        {
            try
            {
                database.GetCollection<T>(typeof(T).Name).InsertOne(entity);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class, ICache, new()
        {
            try
            {
                database.GetCollection<T>(typeof(T).Name).InsertMany(entities);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Count<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable().Count(func);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Count<T>() where T : class, ICache, new()
        {
            try
            {
                return (int)database.GetCollection<T>(typeof(T).Name).CountDocuments(en => true);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T Find<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable().FirstOrDefault(func);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T Last<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable().LastOrDefault(func);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable().Where(func);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<T> Query<T>() where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable();
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<R> Select<T, R>(Expression<Func<T, R>> func) where T : class, ICache, new()
        {
            try
            {
                return database.GetCollection<T>(typeof(T).Name).AsQueryable().Select(func);
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Remove<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return (int)database.GetCollection<T>(typeof(T).Name).DeleteOne(func).DeletedCount;
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int RemoveRange<T>(Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                return (int)database.GetCollection<T>(typeof(T).Name).DeleteMany(func).DeletedCount;
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Update<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                var entity = this.Find<T>(func);
                action?.Invoke(entity);
                return (int)database.GetCollection<T>(typeof(T).Name).ReplaceOne(func, entity).ModifiedCount;
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int UpdateRange<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, ICache, new()
        {
            try
            {
                var entities = this.Query<T>(func);
                int count = 0;
                foreach (T en in entities)
                {
                    action?.Invoke(en);
                    count += (int)database.GetCollection<T>(typeof(T).Name).ReplaceOne(func, en).ModifiedCount;
                }
                return count;
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }


        private SortDefinition<T> getSortDefinition<T>(Dictionary<string, int> sortfields)
        {
            SortDefinition<T> sd = null;
            foreach (var item in sortfields)
            {
                if (sd == null)
                {
                    if (item.Value == 1)
                        sd = Builders<T>.Sort.Ascending(item.Key);
                    else
                        sd = Builders<T>.Sort.Descending(item.Key);
                }
                else
                {
                    if (item.Value == 1)
                        sd.Ascending(item.Key);
                    else
                        sd.Descending(item.Key);
                }
            }
            return sd;
        }
        private UpdateDefinition<T> getUpdateDefinition<T>(Dictionary<string, object> updatedic)
        {
            UpdateDefinition<T> ud = null;
            foreach (var item in updatedic)
            {
                if (ud == null)
                    ud = Builders<T>.Update.Set(item.Key, item.Value);
                else
                    ud.Set(item.Key, item.Value);
            }
            return ud;
        }
    }
}
