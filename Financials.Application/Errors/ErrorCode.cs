namespace Financials.Application.Errors
{
    public enum ErrorCode
    {
        NA = 0,
        Validation = 100,
        EmailNotVerified = 150,
        InvalidEmailOrPassword = 200,
        InvalidFederationCode = 210,
        UserNotFound = 250
    }
}
