using Financials.Application.Configuration;
using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionQueryDecorator<TQuery, TResult> : QueryDecorator<TQuery, TResult> where TQuery : IQuery<TResult>, IRequirePermission
    {
        private readonly IAccess access;
        private readonly AppSettings appSettings;

        public RequirePermissionQueryDecorator(IQueryHandler<TQuery, TResult> queryHandler, IAccess access, AppSettings appSettings) :
            base(queryHandler)
        {
            this.access = access;
            this.appSettings = appSettings;
        }
        public override Task<Result<TResult>> Handle(TQuery query)
        {
            var perm = query as IRequirePermission;
            if (perm == null)
                throw new Exception($"Command must implement {typeof(IRequirePermission)}");

            if (access.CanDo(perm.TenantId, appSettings.ApplicationName, perm.Permission.ToString()))
            {
                return queryHandler.Handle(query);
            }
            else
            {
                return Result<TResult>.Fail(CommandError.Forbidden($"Permission denied for {perm.Permission}")).AsTaskTyped();
            }
        }
    }
}
