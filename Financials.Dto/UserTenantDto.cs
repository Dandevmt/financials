using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class UserTenantDto
    {
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public DateTime? Federated { get; set; }
        public HashSet<string> Permissions { get; set; }
    }
}
