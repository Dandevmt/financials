using Financials.Application;
using Financials.CQRS;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Database
{
    public class UnitOfWorkDecorator<TCommand, TResult> : CommandDecorator<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly IClientSessionHandle session;
        
        public UnitOfWorkDecorator(
            ICommandHandler<TCommand, TResult> handler, 
            IClientSessionHandle session) : base(handler)
        {
            this.session = session;
        }

        public override Result<TResult> Handle(TCommand input)
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
