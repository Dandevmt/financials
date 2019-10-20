using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Financials.Application.CQRS
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        CommandResult Handle(TCommand command);
    }
}
