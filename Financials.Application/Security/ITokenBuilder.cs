namespace Financials.Application.Security
{
    public interface ITokenBuilder
    {
        public string Build(Entities.User user);
    }
}