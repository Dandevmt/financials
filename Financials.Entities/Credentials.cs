using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.Entities
{
    public class Credentials
    {
        public ObjectId Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime EmailVerified { get; set; }
    }
}
