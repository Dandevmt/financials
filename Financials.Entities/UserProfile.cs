using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
    }
}
