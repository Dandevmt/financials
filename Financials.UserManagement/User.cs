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
        public DateTime? Archived { get; set; }

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

        public Result ResetPassword(PasswordHashed oldPW, PasswordHashed newPW)
        {
            if (PasswordHashed != oldPW)
                return Result.Fail(ValidationError.New().AddError("Password", "Old Password is Invalid"));

            PasswordHashed = newPW;

            return Result.Success();
        }
    }
}
