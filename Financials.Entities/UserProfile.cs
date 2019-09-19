using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class UserProfile
    {
        public DateTime EmailVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
    }
}
