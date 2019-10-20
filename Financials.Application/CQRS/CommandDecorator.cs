using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Financials.Application.CQRS
{
    public class CommandDecorator<TCommand> where TCommand : ICommand
    {
        protected readonly ICommandHandler<TCommand> commandHandler;

        public CommandDecorator(ICommandHandler<TCommand> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        protected ICommandHandler<TCommand> GetDecoratedCommand()
        {
            var handler = commandHandler;
            while (handler.GetType().IsAssignableFrom(typeof(CommandDecorator<TCommand>)))
            {
                handler = (ICommandHandler<TCommand>)handler.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(f => f.FieldType.IsAssignableFrom(typeof(ICommandHandler<TCommand>)))?.GetValue(handler);
            }
            return handler;
        }
    }
}
