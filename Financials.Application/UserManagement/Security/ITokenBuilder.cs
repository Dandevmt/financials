namespace Financials.Application.UserManagement.Security
{
    public interface ITokenBuilder
    {
        public string Build(Entities.User user);
    }
}