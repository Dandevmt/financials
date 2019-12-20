using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.CQRS
{
    public abstract class CommandDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand
    {
        protected readonly ICommandHandler<TCommand, TResult> commandHandler;

        public CommandDecorator(ICommandHandler<TCommand, TResult> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public abstract Result<TResult> Handle(TCommand command);
    }
}
