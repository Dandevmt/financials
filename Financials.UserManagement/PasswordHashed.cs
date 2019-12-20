using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class PasswordHashed
    {
        public string Value { get; private set; }

        private PasswordHashed(string value)
        {
            Value = value;
        }

        public static Result<PasswordHashed> Create(string password, string passwordVerified, IHasherService hasher)
        {
            if (password != passwordVerified)
                return Result<PasswordHashed>.Fail(null, ValidationError.New().AddError("Password", "Passwords do not match"));

            return Result<PasswordHashed>.Success(new PasswordHashed(hasher.HashPassword(password)));
        }

        public override bool Equals(object obj)
        {
            var pw = obj as PasswordHashed;

            if (pw == null)
                return false;

            return Equals(pw);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(PasswordHashed password)
        {
            if (password == null)
                return false;

            return password.Value == this.Value;
        }
    }
}
