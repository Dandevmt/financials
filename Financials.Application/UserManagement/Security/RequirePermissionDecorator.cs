using Financials.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Security
{
    public class RequirePermissionDecorator<TCommand> : CommandDecorator<TCommand>, ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IAccess access;

        public RequirePermissionDecorator(ICommandHandler<TCommand> commandHandler, IAccess access): 
            base(commandHandler)
        {
            this.access = access;
        }
        public async Task<CommandResult> Handle(TCommand command)
        {
            var originalHandler = GetDecoratedCommand();
            var attr = originalHandler.GetType().GetCustomAttribute<RequirePermissionAttribute>();
            if (attr == null)
                throw new Exception($"Could not find attribute of type {typeof(RequirePermissionAttribute)}");
            
            if (access.CanDo(attr.Permission))
            {
                return await commandHandler.Handle(command);
            }
            else
            {
                return CommandResult.Fail(CommandError.Forbidden($"Permission denied for {attr.Permission}"));
            }
        }
    }
}
