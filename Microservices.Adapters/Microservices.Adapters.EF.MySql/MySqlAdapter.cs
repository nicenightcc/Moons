using Microservices.Adapters.IDatabase;
using Microservices.Base;
using Microservices.Common;
using Microservices.IoC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservices.Adapters.EF.MySql
{
    public class MySqlAdapter : ISqlAdapter
    {
        private BlockingCollection<DbContext> contextpool;

        public string Name { get => "MySqlAdapter"; }
        public Type TargetType { get => typeof(ISqlAdapter); }
        public MySqlAdapter()
        {
            string connstr = Config.Root["MySql"];
            if (string.IsNullOrEmpty(connstr))
                throw new Exception("Config Not Found: [MySql]");
            var tables = IoCFac.Instance.GetClassList<IEntity>().ToArray();

            string maxconn = Config.Root["MySqlMaxConn"];
            var contextcount = maxconn == null ? 10 : int.Parse(maxconn);
            this.contextpool = new BlockingCollection<DbContext>(contextcount);

            for (var i = 0; i < contextpool.BoundedCapacity; i++)
                contextpool.Add(new MySqlContext(connstr, tables));
        }

        public T Add<T>(T entity) where T : class, IEntity, new()
        {
            try
            {
                var context = contextpool.Take();
                entity = context.Set<T>().Add(entity).Entity;
                context.SaveChanges();
                contextpool.Add(context);
                return entity;
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
                var context = contextpool.Take();
                context.Set<T>().AddRange(entities);
                var result = context.SaveChanges();
                contextpool.Add(context);
                return result;
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
                var context = contextpool.Take();
                var result = context.Set<T>().Count(func);
                contextpool.Add(context);
                return result;
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
                var context = contextpool.Take();
                var result = context.Set<T>().Count();
                contextpool.Add(context);
                return result;
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
                var context = contextpool.Take();
                entity = context.Set<T>().Remove(entity).Entity;
                context.SaveChanges();
                contextpool.Add(context);
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
                var context = contextpool.Take();
                var entitys = context.Set<T>().Where(func);
                context.Set<T>().RemoveRange(entitys);
                var result = context.SaveChanges();
                contextpool.Add(context);
                return result;
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
                var entype = typeof(T).GetProperties().Where(en => en.CustomAttributes.Any(attr => attr.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.ForeignKeyAttribute))).Select(en => en.PropertyType);

                var context = contextpool.Take();
                var result = context.Set<T>().FirstOrDefault(func);
                contextpool.Add(context);
                return result;
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
                var context = contextpool.Take();
                var result = context.Set<T>().LastOrDefault(func);
                contextpool.Add(context);
                return result;
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
                return new QueryProvider<T>(contextpool).CreateQuery().Where(func);
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
                return new QueryProvider<T>(contextpool).CreateQuery();
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
                return new QueryProvider<T>(contextpool).CreateQuery().Select(func);
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
                var context = contextpool.Take();
                var entity = context.Set<T>().First(func);
                if (entity == null) return 0;
                context.Update(entity);
                action.Invoke(entity);
                var result = context.SaveChanges();
                contextpool.Add(context);
                return result;
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
                var context = contextpool.Take();
                var entitys = context.Set<T>().Where(func);
                if (entitys.Count() == 0) return 0;
                context.UpdateRange(entitys);
                foreach (T en in entitys)
                    action.Invoke(en);
                var result = context.SaveChanges();
                contextpool.Add(context);
                return result;
            }
            catch (Exception e)
            {
                throw new AdapterException(e.InnerException?.Message ?? e.Message);
            }
        }

    }
}
