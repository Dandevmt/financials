using Financials.CQRS;
using MongoDB.Bson;
using System;

namespace Financials.UserManagement
{
    public class User
    {
        public ObjectId Id { get; private set; }
        public string Username { get; private set; }
        public Email Email { get; private set; }
        public PasswordHashed PasswordHashed { get; private set; }
        public ResetPasswordCode ResetPasswordCode { get; private set; }
        public Profile Profile { get; private set; }
        public DateTime? ArchivedDate { get; set; }
        public bool Archived { get { return ArchivedDate != null; } }

        public static Result<User> Create(string username, Email email, PasswordHashed password)
        {
            // Username must be > 5 characters
            if (string.IsNullOrWhiteSpace(username) || username.Length < 5)
                return Result<User>.Fail(null, ValidationError.New().AddError("Username", "Username must be at least 5 characters"));

            return Result<User>.Success(new User()
            {
                Username = username,
                Email = email,
                PasswordHashed = password
            });
        }

        public void UpdateEmail(Email email)
        {
            Email = email;
        }

        public void UpdateResetPasswordCode(ResetPasswordCode resetPasswordCode)
        {
            ResetPasswordCode = resetPasswordCode;
        }

        public Result ResetPassword(string code, PasswordHashed newPW)
        {
            if (ResetPasswordCode?.Expiration < DateTime.Now || ResetPasswordCode.Code != code)
                return Result.Fail(Errors.InvalidResetPasswordCode());

            PasswordHashed = newPW;
            return Result.Success();
        }

        public Result ChangePassword(PasswordHashed oldPW, PasswordHashed newPW)
        {
            if (PasswordHashed != oldPW)
                return Result.Fail(ValidationError.New().AddError("Password", "Old Password is Invalid"));

            PasswordHashed = newPW;

            return Result.Success();
        }

        public bool VerifyEail(string code)
        {
            return Email.Verify(code);
        }

        public void UpdateProfile(Profile profile)
        {
            Profile = profile;
        }

        public void Archive()
        {
            ArchivedDate = DateTime.Now;
        }
    }
}
