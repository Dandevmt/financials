using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionDecorator<TCommand> : CommandDecorator<TCommand> where TCommand : ICommand, IRequirePermission
    {
        private readonly IAccess access;

        public RequirePermissionDecorator(ICommandHandler<TCommand> commandHandler, IAccess access): 
            base(commandHandler)
        {
            this.access = access;
        }
        public override Task<CommandResult> Handle(TCommand command)
        {
            var perm = command as IRequirePermission;
            if (perm == null)
                throw new Exception($"Command must implement {typeof(IRequirePermission)}");
            
            if (access.CanDo(perm.TenantId, perm.Permission))
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
