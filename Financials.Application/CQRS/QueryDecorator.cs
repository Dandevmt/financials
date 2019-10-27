using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.CQRS
{
    public abstract class QueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        protected readonly IQueryHandler<TQuery, TResult> queryHandler;

        public QueryDecorator(IQueryHandler<TQuery, TResult> queryHandler)
        {
            this.queryHandler = queryHandler;
        }

        public abstract Task<CommandResult<TResult>> Handle(TQuery query);

        protected IQueryHandler<TQuery, TResult> GetDecoratedQuery()
        {
            var handler = queryHandler;
            while (handler.GetType().IsAssignableFrom(typeof(QueryDecorator<TQuery, TResult>)))
            {
                handler = (IQueryHandler<TQuery, TResult>)handler.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(f => f.FieldType.IsAssignableFrom(typeof(IQueryHandler<TQuery, TResult>)))?.GetValue(handler);
            }
            return handler;
        }
    }
}
