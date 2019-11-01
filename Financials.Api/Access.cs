using Financials.Application.UserManagement.Repositories;
using Financials.Application.UserManagement.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Financials.Api
{
    public class Access : IAccess
    {
        private readonly IUserRepository userRepo;
        private readonly Guid userId;
        private Entities.User user;
        public Access(IHttpContextAccessor context, IUserRepository userRepo)
        {
            if (context == null || context.HttpContext == null || context.HttpContext.User == null)
                return;
            this.userRepo = userRepo;
            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                Guid.TryParse(claim.Value, out userId);
            }
        }

        public Entities.User CurrentUser()
        {
            if (userId == null)
                return null;

            if (user == null)
            {
                user = userRepo.Get(userId);
            }

            return user;
        }

        public bool CanDo(string tenantId, Permission permission)
        {
            user = CurrentUser();
            var permissions = user?.Tenants?.FirstOrDefault(t => t.TenantId == tenantId)?.Permissions;

            if (permissions == null || permissions.Count == 0)
                return false;

            return permissions.Contains(permission.ToString());
        }
    }
}
