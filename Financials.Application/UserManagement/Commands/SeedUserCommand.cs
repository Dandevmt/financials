﻿using Financials.Application.CQRS;
using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Financials.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.UserManagement.Commands
{
    public class SeedUserCommand : ICommand
    {
        public string Email { get; }
        public string Password { get; }

        public SeedUserCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class SeedUserCommandHandler : ICommandHandler<SeedUserCommand>
    {
        private readonly ITenantRepository tenantRepo;
        private readonly IUserRepository userRepo;
        private readonly IPasswordHasher hasher;

        public SeedUserCommandHandler(ITenantRepository tenantRepo, IUserRepository userRepo, IPasswordHasher hasher)
        {
            this.tenantRepo = tenantRepo;
            this.userRepo = userRepo;
            this.hasher = hasher;
        }

        public Task<CommandResult> Handle(SeedUserCommand command)
        {
            var user = userRepo.Get(command.Email);
            if (user != null)
                return CommandResult.Success().AsTask();

            var tenant = tenantRepo.Add(new Tenant() { TenantId = "admin", TenantName = "Admin" });

            user = new User() 
            {
                Credentials = new Credentials() 
                {
                    Email = command.Email,
                    EmailVerified = DateTime.Now,
                    Password = hasher.HashPassword(command.Password)
                },
                Profile = new UserProfile() 
                {
                    FirstName = "Fin User",
                    LastName = "Extreme",
                    Address  = new Address()
                },
                Tenants = new List<UserTenant>() 
                { 
                    new UserTenant() 
                    { 
                        TenantId = tenant.TenantId,
                        Federated = DateTime.Now,
                        FederationCode = "autogenerated",
                        Permissions = new HashSet<string>() { Permission.AddUsers.ToString(), Permission.DeleteUser.ToString(), Permission.EditUsers.ToString(), Permission.ViewUsers.ToString() }
                    } 
                },
                Registered = true,                
                ValidationCodes = new List<ValidationCode>()
            };
            userRepo.Add(user);

            return CommandResult.Success().AsTask();
        }
    }
}
