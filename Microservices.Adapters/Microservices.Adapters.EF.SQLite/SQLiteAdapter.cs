using Microservices.Adapters.IDatabase;
using Microservices.Base;
using Microservices.Common;
using Microservices.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservices.Adapters.EF.SQLite
{
    public class SQLiteAdapter : ISqlAdapter
    {
        private SQLiteContext context;

        public string Name { get => "SQLiteAdapter"; }
        public Type TargetType { get => typeof(ISqlAdapter); }
        public SQLiteAdapter()
        {
            string connstr = Config.Root["SQLite"];
            if (string.IsNullOrEmpty(connstr))
                throw new Exception("Config Not Found: [SQLite]");

            var tables = IoCFac.Instance.GetClassList<IEntity>()?.ToArray();

            this.context = new SQLiteContext(connstr, tables);
        }

        public T Add<T>(T entity) where T : class, IEntity, new()
        {
            try
            {
                entity = context.Set<T>().Add(entity).Entity;
                if (context.SaveChanges() > 0)
                    return entity;
                else
                    return new T();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int AddRange<T>(IEnumerable<T> entities) where T : class, IEntity, new()
        {
            try
            {
                context.Set<T>().AddRange(entities);
                return context.SaveChanges();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Count<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().Count(func);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Count<T>() where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().Count();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T Remove<T>(T entity) where T : class, IEntity, new()
        {
            try
            {
                entity = context.Set<T>().Remove(entity).Entity;
                context.SaveChanges();
                return entity;
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int RemoveRange<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                var entitys = context.Set<T>().Where(func);
                context.Set<T>().RemoveRange(entitys);
                return context.SaveChanges();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T Find<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().FirstOrDefault(func);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public T Last<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().LastOrDefault(func);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().Where(func);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<T> Query<T>() where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().AsQueryable();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public IQueryable<R> Select<T, R>(Expression<Func<T, R>> func) where T : class, IEntity, new()
        {
            try
            {
                return context.Set<T>().Select(func);
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int Update<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                var entity = ((ISqlAdapter)this).Find(func);
                if (entity == null) return 0;
                context.Update(entity);
                action?.Invoke(entity);
                return context.SaveChanges();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

        public int UpdateRange<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, IEntity, new()
        {
            try
            {
                var entitys = ((ISqlAdapter)this).Query(func);
                if (entitys.Count() == 0) return 0;
                context.UpdateRange(entitys);
                foreach (T en in entitys)
                {
                    action?.Invoke(en);
                }
                return context.SaveChanges();
            }
            catch (Exception e)
            {
               throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }
    }
}
