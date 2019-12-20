using Financials.Common.Email;
using System.Threading.Tasks;

namespace Financials.UserManagement
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message);
    }
}