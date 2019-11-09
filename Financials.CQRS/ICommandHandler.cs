using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Financials.CQRS
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<CommandResult> Handle(TCommand command);
    }

}
