using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Queries
{
    public class GetTenantQuery : IQuery<TenantDto>
    {
        public string Id { get; }

        public GetTenantQuery(string id)
        {
            Id = id;
        }
    }

    public class GetTenantQueryHandler : IQueryHandler<GetTenantQuery, TenantDto>
    {
        private readonly ITenantRepository tenantRepo;

        public GetTenantQueryHandler(ITenantRepository tenantRepo)
        {
            this.tenantRepo = tenantRepo;
        }

        public Task<CommandResult<TenantDto>> Handle(GetTenantQuery query)
        {
            var tenant = tenantRepo.Get(query.Id);
            if (tenant == null)
            {
                return CommandResult<TenantDto>.Fail(UserManagementError.TenanNotFound()).AsTaskTyped();
            }
            var dto = new TenantDto()
            {
                Id = tenant.TenantId.ToString(),
                Name = tenant.TenantName
            };
            return CommandResult<TenantDto>.Success(dto).AsTaskTyped();
        }
    }
}
