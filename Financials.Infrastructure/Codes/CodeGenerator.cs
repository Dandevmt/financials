using Financials.Application.Codes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Financials.Infrastructure.Codes
{
    public class CodeGenerator : ICodeGenerator
    {
        public string Generate(int byteSize = 16)
        {
            byte[] codeBytes = new byte[byteSize];
            RandomNumberGenerator.Create().GetBytes(codeBytes);
            string code = Convert.ToBase64String(codeBytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return code;
        }
    }
}
