namespace Financials.Application.Security.UseCases
{
    public interface ITokenBuilder
    {
        public string Build(Entities.User user);
    }
}