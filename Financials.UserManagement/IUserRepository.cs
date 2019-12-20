namespace Financials.UserManagement
{
    public interface IUserRepository
    {
        User GetByUsername(string username);
        User Add(User user);
    }
}