using Microservices.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservices.Adapters.IDatabase
{
    public interface ISqlAdapter : IAdapter
    {
        T Add<T>(T entity) where T : class, IEntity, new();

        int AddRange<T>(IEnumerable<T> entities) where T : class, IEntity, new();

        int Count<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new();

        int Count<T>() where T : class, IEntity, new();

        T Remove<T>(T entity) where T : class, IEntity, new();

        int RemoveRange<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new();

        T Find<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new();

        T Last<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new();

        IQueryable<T> Query<T>(Expression<Func<T, bool>> func) where T : class, IEntity, new();

        IQueryable<T> Query<T>() where T : class, IEntity, new();

        IQueryable<R> Select<T, R>(Expression<Func<T, R>> func) where T : class, IEntity, new();

        int Update<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, IEntity, new();

        int UpdateRange<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, IEntity, new();
    }
}
