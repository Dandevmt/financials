using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class Address
    {
        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }
        public string Country { get; private set; }

        public static Address Create(string line1, string city, string state, string zip, string country)
        {
            return new Address() 
            {
                Line1 = line1,
                City = city,
                State = state,
                Zip = zip, 
                Country = country
            };
        }

        public static Address Create(string line1, string line2, string city, string state, string zip, string country)
        {
            var addr = Create(line1, city, state, zip, country);
            addr.Line2 = line2;
            return addr;
        }
    }
}
