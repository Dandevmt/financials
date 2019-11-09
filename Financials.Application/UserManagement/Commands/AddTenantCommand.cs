using Financials.Application.UserManagement.Repositories;
using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class AddTenantCommand : ICommand
    {
        public string Id { get; }
        public string Name { get; }
        public AddTenantCommand(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class AddTenantCommandHandler : ICommandHandler<AddTenantCommand>
    {
        private readonly ITenantRepository tenantRepo;

        public AddTenantCommandHandler(ITenantRepository tenantRepo)
        {
            this.tenantRepo = tenantRepo;
        }

        public Task<CommandResult> Handle(AddTenantCommand command)
        {
            var existinTenant = tenantRepo.Get(command.Id);
            if (existinTenant != null)
                return CommandResult.Fail(UserManagementError.TenantCodeAlreadyExists()).AsTask();

            var tenant = tenantRepo.Add(new Entities.Tenant()
            {
                TenantId = command.Id,
                TenantName = command.Name
            });
            return CommandResult<string>.Success(tenant.TenantId.ToString()).AsTask();
        }
    }
}
