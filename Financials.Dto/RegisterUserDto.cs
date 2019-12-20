using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class RegisterUserDto
    {
        public string Password { get; set; }
        public string Password2 { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
    }
}
