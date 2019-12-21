using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class ResetPasswordCode
    {
        public string Code { get; }
        public DateTime Expiration { get; }

        private ResetPasswordCode(string code)
        {
            Code = code;
            Expiration = DateTime.Now.AddMinutes(60);
        }

        public static ResetPasswordCode Create(ICodeGenerator codeGenerator)
        {
            return new ResetPasswordCode(codeGenerator.Generate(12));
        }
    }
}
