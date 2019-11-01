using System;
using System.Collections.Generic;

namespace Financials.Dto
{
    public class UserForUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public string Email { get; set; }
        public DateTime? EmailVerified { get; set; }
        public List<UserTenantDto> Tenants { get; set; }
        public AddresssDto Address { get; set; }
    }
}
