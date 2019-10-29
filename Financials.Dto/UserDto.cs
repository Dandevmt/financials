using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? EmailVerified { get; set; }
        public bool Registered { get; set; }
        public HashSet<string> Permissions { get; set; }
        public AddresssDto Address { get; set; }
        public string FederationCode { get; set; }
        public DateTime? FederationCodeExpiration { get; set; }
    }
}
