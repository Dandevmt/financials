using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class RegisterUserDto : AddUserDto
    {
        public string Password { get; set; }
        public string Password2 { get; set; }
    }
}
