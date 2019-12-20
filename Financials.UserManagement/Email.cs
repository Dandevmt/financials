using Financials.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financials.UserManagement
{
    public class Email
    {
        public string Value { get; }
        public string VerificationCode { get; set; }
        public DateTime? Verified { get; set; }

        private Email(string email, string verificationCode)
        {
            Value = email;
            VerificationCode = verificationCode;
        }

        public static Result<Email> Create(string email, string emailVerified, ICodeGenerator codeGenerator)
        {
            var errors = ValidationError.New();

            if (!email.Contains("@"))
                errors.AddError("Email", "Invalid Email Format");

            if (email != emailVerified)
                errors.AddError("Email", "Emails do not match");

            if (errors.HasError)
                return Result<Email>.Fail(null, errors);

            return Result<Email>.Success(new Email(email, codeGenerator.Generate()));
        }

        public bool Verify(string verificationCode)
        {
            if (VerificationCode == verificationCode)
            {
                Verified = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }            
        }

        public bool IsVerified()
        {
            return Verified != null;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }
    }
}
