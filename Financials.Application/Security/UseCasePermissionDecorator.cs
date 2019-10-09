using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var uc = useCase;
            while (uc != null)
            {                
                if (uc is IPermissionRequired permUseCase)
                {
                    if (access.CanDo(permUseCase.PermissionRequired))
                    {
                        useCase.Handle(input, presenter);
                        return;
                    }
                    else
                    {
                        Errors.ForbiddenError.Throw(permUseCase.PermissionRequired);
                    }
                }
                uc = (IUseCase<TInput, TOutput>)useCase.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(p => p.FieldType.IsAssignableFrom(typeof(IUseCase<TInput, TOutput>))).GetValue(useCase);
            }
            {
                throw new Exception($"Cannot assign {useCase.GetType()} to {typeof(IPermissionRequired)}");
            }
        }
    }
}
