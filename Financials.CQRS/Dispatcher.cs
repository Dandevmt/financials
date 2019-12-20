using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.CQRS
{
    public class Dispatcher
    {
        private readonly IProvider provider;
        private static IList<IEvent> events;

        public Dispatcher(IProvider provider)
        {
            this.provider = provider;
            events = new List<IEvent>();
        }

        public Result<TResult> Command<TResult>(ICommand<TResult> command)
        {
            Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
            var handler = provider.GetService(handlerType) as dynamic;
            if (handler == null)
                throw new Exception($"Could not get service of type {handlerType}");

            return handler.Handle((dynamic)command);
        }

        public async Task<Result<TResult>> Query<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TQuery), typeof(TResult));
            var handler = provider.GetService(handlerType) as IQueryHandler<TQuery, TResult>;
            if (handler == null)
                throw new Exception($"Could not get service of type {handlerType}");

            return await handler.Handle(query);
        }

        public void QueueEvent(IEvent evnt)
        {
            events.Add(evnt);
        }

        public async Task ProcessEvents()
        {
            var handlers = new List<Task>();
            foreach(var evnt in events)
            {
                Type handlerType = typeof(IEventHandler<>).MakeGenericType(evnt.GetType());
                var handler = provider.GetService(handlerType);
                if (handler == null)
                    throw new Exception($"Could not get service of type {handlerType}");
                handlers.Add(((dynamic)handler).Handle((dynamic)evnt));
            }
            events.Clear();
            await Task.WhenAll(handlers.ToArray());            
        }
        
    }
}
