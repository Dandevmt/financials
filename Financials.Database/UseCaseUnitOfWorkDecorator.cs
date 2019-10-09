using Financials.Application;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Database
{
    public class UseCaseUnitOfWorkDecorator<TInput, TOutput> : IUseCase<TInput, TOutput>
    {
        private readonly IClientSessionHandle session;
        private readonly IUseCase<TInput, TOutput> useCase;
        
        public UseCaseUnitOfWorkDecorator(IClientSessionHandle session, IUseCase<TInput, TOutput> useCase)
        {
            this.session = session;
            this.useCase = useCase;
        }

        public void Handle(TInput input, Action<TOutput> presenter)
        {
            using (session)
            {
                // session.StartTransaction();
                try
                {
                    useCase.Handle(input, presenter);
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
