namespace Financials.Application.Errors
{
    public enum ErrorCode
    {
        NA = 0,
        Validation = 100,
        EmailNotVerified = 150,
        EmailCouldNotUpdateDatabase = 160,
        InvalidEmailOrPassword = 200,
        InvalidEmailVerificationCode = 250,
        InvalidFederationCode = 210,
        UserNotFound = 250,
        Forbidden = 403
    }
}
