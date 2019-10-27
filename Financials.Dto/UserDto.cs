using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public HashSet<string> Permissions { get; set; }
        public AddresssDto Address { get; set; }
    }
}
