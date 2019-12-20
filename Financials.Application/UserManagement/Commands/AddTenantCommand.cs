using Financials.Application.UserManagement.Repositories;
using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class AddTenantCommand : ICommand<string>
    {
        public string Id { get; }
        public string Name { get; }
        public AddTenantCommand(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class AddTenantCommandHandler : ICommandHandler<AddTenantCommand, string>
    {
        private readonly ITenantRepository tenantRepo;

        public AddTenantCommandHandler(ITenantRepository tenantRepo)
        {
            this.tenantRepo = tenantRepo;
        }

        public Result<string> Handle(AddTenantCommand command)
        {
            var existinTenant = tenantRepo.Get(command.Id);
            if (existinTenant != null)
                return Result.Fail(UserManagementError.TenantCodeAlreadyExists()).AsTask();

            var tenant = tenantRepo.Add(new Entities.Tenant()
            {
                TenantId = command.Id,
                TenantName = command.Name
            });
            return Result<string>.Success(tenant.TenantId.ToString());
        }
    }
}
