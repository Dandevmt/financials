using Financials.Application;
using Financials.Application.CQRS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Database
{
    public class UnitOfWorkDecorator<TCommand> : CommandDecorator<TCommand> where TCommand : ICommand
    {
        private readonly IClientSessionHandle session;
        
        public UnitOfWorkDecorator(
            ICommandHandler<TCommand> handler, 
            IClientSessionHandle session) : base(handler)
        {
            this.session = session;
        }

        public override Task<CommandResult> Handle(TCommand input)
        {
            using (session)
            {
                // session.StartTransaction();
                try
                {
                    return commandHandler.Handle(input);
                    // session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    //session.AbortTransaction();
                    throw ex;
                }
            }                       
        }
    }
}
