using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Security
{
    public class UseCasePermissionDecorator<TInput, TOutput> : IUseCase<TInput, TOutput>
    {        
        private readonly IUseCase<TInput, TOutput> useCase;
        private readonly IAccess access;

        public UseCasePermissionDecorator(IUseCase<TInput, TOutput> useCase, IAccess access)
        {
            this.useCase = useCase;
            this.access = access;
        }        

        public void Handle(TInput input, Action<TOutput> presenter)
        {
            if (useCase is IPermissionRequired permUseCase)
            {
                if (access.CanDo(permUseCase.PermissionRequired))
                {
                    useCase.Handle(input, presenter);
                } else
                {
                    throw new UnauthorizedAccessException($"Access denied for: {permUseCase.PermissionRequired}");
                }
            } else
            {
                throw new Exception($"Cannot assign {useCase.GetType()} to {typeof(IPermissionRequired)}");
            }
        }
    }
}
