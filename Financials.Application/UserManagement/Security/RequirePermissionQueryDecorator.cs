using Financials.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionQueryDecorator<TQuery, TResult> : QueryDecorator<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IAccess access;

        public RequirePermissionQueryDecorator(IQueryHandler<TQuery, TResult> queryHandler, IAccess access) :
            base(queryHandler)
        {
            this.access = access;
        }
        public override Task<CommandResult<TResult>> Handle(TQuery query)
        {
            var originalHandler = GetDecoratedQuery();
            var attr = originalHandler.GetType().GetCustomAttribute<RequirePermissionAttribute>();
            if (attr == null)
                throw new Exception($"Could not find attribute of type {typeof(RequirePermissionAttribute)}");

            if (access.CanDo(attr.Permission))
            {
                return queryHandler.Handle(query);
            }
            else
            {
                return CommandResult<TResult>.Fail(CommandError.Forbidden($"Permission denied for {attr.Permission}")).AsTaskTyped();
            }
        }
    }
}
