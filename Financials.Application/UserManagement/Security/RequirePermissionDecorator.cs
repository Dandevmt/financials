using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Financials.Application.Configuration;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionDecorator<TCommand> : CommandDecorator<TCommand> where TCommand : ICommand, IRequirePermission
    {
        private readonly AppSettings appSettings;
        private readonly IAccess access;

        public RequirePermissionDecorator(ICommandHandler<TCommand> commandHandler, IAccess access, AppSettings appSettings): 
            base(commandHandler)
        {
            this.access = access;
            this.appSettings = appSettings;
        }
        public override Task<CommandResult> Handle(TCommand command)
        {
            var perm = command as IRequirePermission;
            if (perm == null)
                throw new Exception($"Command must implement {typeof(IRequirePermission)}");
            
            if (access.CanDo(perm.TenantId, appSettings.ApplicationName, perm.Permission.ToString()))
            {
                return commandHandler.Handle(command);
            }
            else
            {
                return CommandResult.Fail(CommandError.Forbidden($"Permission denied for {perm.Permission}")).AsTask();
            }
        }
    }
}
