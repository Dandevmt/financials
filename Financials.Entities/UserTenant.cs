using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class UserTenant
    {
        public string TenantId { get; set; }
        public string FederationCode { get; set; }
        public DateTime? Federated { get; set; }
        public Dictionary<string, HashSet<string>> Permissions { get; set; }
    }
}
