using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Application
{
    public static class ObjectExtensions
    {
        public static T StaticCast<T>(this T o) => o;
    }
}
