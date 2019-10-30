using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class AddTenantCommand : ICommand
    {
        public string Name { get; }
        public AddTenantCommand(string name)
        {
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
            var tenant = tenantRepo.Add(new Entities.Tenant()
            {
                TenantName = command.Name
            });
            return CommandResult<string>.Success(tenant.TenantId.ToString()).AsTask();
        }
    }
}
