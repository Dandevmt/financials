using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class AddTenantToUserCommand : ICommand
    {
        public string TenantId { get; }
        public string FederationCode { get; }
        public bool ValidateOnly { get; }

        public AddTenantToUserCommand(string tenantId, string federationCode, bool validateOnly)
        {
            TenantId = tenantId;
            FederationCode = federationCode;
            ValidateOnly = validateOnly;
        }
    }

    public class AddTenantToUserCommandHandler : ICommandHandler<AddTenantToUserCommand>
    {
        private readonly IAccess access;
        private readonly ITenantRepository tenantRepo;
        private readonly IUserRepository userRepo;

        public AddTenantToUserCommandHandler(IAccess access, ITenantRepository tenantRepo, IUserRepository userRepo)
        {
            this.access = access;
            this.tenantRepo = tenantRepo;
            this.userRepo = userRepo;
        }

        public Task<Result> Handle(AddTenantToUserCommand command)
        {
            var user = access.CurrentUser();
            if (user == null)
                return Result.Fail(UserManagementError.UserNotLoggedIn()).AsTask();

            var tenant = tenantRepo.Get(command.TenantId);
            if (tenant == null)
                return Result.Fail(ValidationError.New().AddError(nameof(command.TenantId), "Invalid Organization Id")).AsTask();

            if (command.ValidateOnly)
                return Result.Success().AsTask();

            if (user.Tenants.Any(t => t.TenantId == command.TenantId))
                return Result.Success().AsTask();

            user.Tenants.Add(new Entities.UserTenant() 
            {
                TenantId = command.TenantId,
                Permissions = new Dictionary<string, HashSet<string>>()               
            });

            userRepo.Update(user);

            return Result.Success().AsTask();

        }
    }
}
