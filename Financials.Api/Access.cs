using Financials.Application.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financials.Api
{
    public class Access : IAccess
    {
        public bool CanDo(Permission permission)
        {
            return true;
        }
    }
}
