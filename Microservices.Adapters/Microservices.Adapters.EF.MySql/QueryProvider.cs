using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microservices.Adapters.EF
{
    public class QueryProvider<TEntity> : IAsyncQueryProvider, IQueryProvider where TEntity : class
    {
        private BlockingCollection<DbContext> pool;
        public QueryProvider(BlockingCollection<DbContext> pool)
        {
            this.pool = pool;
        }

        public IQueryable<TEntity> CreateQuery()
        {
            return new EntityQueryable<TEntity>(this);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return this.CreateQuery(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new EntityQueryable<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return this.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var context = pool.Take();
            //var entype = typeof(TEntity).GetProperties().Where(en => en.CustomAttributes.Any(attr => attr.AttributeType == typeof(ForeignKeyAttribute))).Select(en => en.PropertyType);

            var provider = context.Set<TEntity>().AsQueryable().Provider as EntityQueryProvider;
            var result = provider.Execute<TResult>(expression);
            pool.Add(context);
            return result;
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            var context = pool.Take();
            var provider = context.Set<TEntity>().AsQueryable().Provider as EntityQueryProvider;
            var result = provider.ExecuteAsync<TResult>(expression);
            pool.Add(context);
            return result;
        }

        public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var context = pool.Take();
            var provider = context.Set<TEntity>().AsQueryable().Provider as EntityQueryProvider;
            var result = await provider.ExecuteAsync<TResult>(expression, cancellationToken);
            pool.Add(context);
            return result;
        }
    }
}
