using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.CQRS
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<CommandResult<TResult>> Handle(TQuery query);
    }
}
