using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.CQRS
{
    public class Dispatcher
    {
        private readonly IProvider provider;

        public Dispatcher(IProvider provider)
        {
            this.provider = provider;
        }

        public async Task<CommandResult> Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            Type handlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(TCommand));
            var handler = provider.GetService(handlerType) as ICommandHandler<TCommand>;
            if (handler == null)
                throw new Exception($"Could not get service of type {handlerType}");

            return await handler.Handle(command);
        }

        public async Task<CommandResult<TResult>> Dispatch<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
            var handler = provider.GetService(handlerType) as IQueryHandler<TQuery, TResult>;
            if (handler == null)
                throw new Exception($"Could not get service of type {handlerType}");

            return await handler.Handle(query);
        }

        
    }
}
