using Microservices.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Microservices.Adapters.IDatabase
{
    public interface IDoctAdapter : IAdapter
    {
        void Add<T>(T entity) where T : class, ICache, new();

        void AddRange<T>(IEnumerable<T> entities) where T : class, ICache, new();

        int Count<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        int Count<T>() where T : class, ICache, new();

        T Find<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        T Last<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        IQueryable<T> Query<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        IQueryable<T> Query<T>() where T : class, ICache, new();

        IQueryable<R> Select<T, R>(Expression<Func<T, R>> func) where T : class, ICache, new();

        int Remove<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        int RemoveRange<T>(Expression<Func<T, bool>> func) where T : class, ICache, new();

        int Update<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, ICache, new();

        int UpdateRange<T>(Action<T> action, Expression<Func<T, bool>> func) where T : class, ICache, new();
    }
}
