namespace Financials.Application.UserManagement.UseCases
{
    public class RegisterUserInput : AddUserInput
    {
        public string Password { get; set; }
        public string FederationCode { get; set; }
    }
}
