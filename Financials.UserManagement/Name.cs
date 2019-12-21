using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class Name
    {
        public string First { get; private set; }
        public string Middle { get; private set; }
        public string Last { get; private set; }

        public static Name Create(string first, string middle, string last)
        {
            return new Name() 
            {
                First = first,
                Middle = middle,
                Last = last
            };
        }
    }
}
