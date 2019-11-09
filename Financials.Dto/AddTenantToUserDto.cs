using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class AddTenantToUserDto
    {
        public string TenantId { get; set; }
        public string FederationCode { get; set; }
        public bool ValidateOnly { get; set; }
    }
}
