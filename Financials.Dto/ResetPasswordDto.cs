using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Dto
{
    public class ResetPasswordDto
    {
        public Guid UserId { get; set; }
        public string ResetCode { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
    }
}
