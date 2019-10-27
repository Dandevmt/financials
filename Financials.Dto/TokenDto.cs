using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class TokenDto
    {
        public string Token { get; set; }
        public int TokenDurationMinutes { get; set; }
        public string Domain { get; set; }
    }
}
