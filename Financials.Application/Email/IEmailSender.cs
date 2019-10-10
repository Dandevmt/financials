using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Financials.Application.Email
{
    public interface IEmailSender
    {
        Task Send(EmailMessage message);
    }
}
