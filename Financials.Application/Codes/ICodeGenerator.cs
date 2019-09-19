using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application.Codes
{
    public interface ICodeGenerator
    {
        string Generate(int byteSize = 16);
    }
}
