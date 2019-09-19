using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Application
{
    public interface IUseCase<TInput, TOutput>
    {
        void Handle(TInput input, Action<TOutput> presenter);
    }
}
