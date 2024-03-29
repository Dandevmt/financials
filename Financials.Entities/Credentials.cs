﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? EmailVerified { get; set; }
    }
}
