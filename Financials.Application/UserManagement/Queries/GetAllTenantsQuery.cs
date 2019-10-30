using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    public class GetAllTenantsQuery : IQuery<IList<TenantDto>>
    {

    }

    public class GetAllTenantsQueryHandler : IQueryHandler<GetAllTenantsQuery, IList<TenantDto>>
    {
        private readonly ITenantRepository tenantRepo;

        public GetAllTenantsQueryHandler(ITenantRepository tenantRepo)
        {
            this.tenantRepo = tenantRepo;
        }

        public Task<CommandResult<IList<TenantDto>>> Handle(GetAllTenantsQuery query)
        {
            var results = tenantRepo.GetAll().Select(t => new TenantDto() 
            { 
                Id = t.TenantId,
                Name = t.TenantName
            }).ToList();
            return CommandResult<IList<TenantDto>>.Success(results).AsTaskTyped();
        }
    }
}
