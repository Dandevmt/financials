using Financials.Application.Email;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Financials.Common.Email;

namespace Financials.Infrastructure.Email
{
    public class EmailSender : UserManagement.IEmailSender
    {
        private readonly SmtpClient client;
        private readonly bool isTest;
        private readonly string smtpPassword;
        private readonly string smtpUsername;
        private readonly string smtpUrl;

        public EmailSender(SmtpClient client, string smtpUrl, string smtpUsername, string smtpPassword, bool isTest = true)
        {
            this.client = client;
            this.isTest = isTest;
            this.smtpUrl = smtpUrl;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }

        public async Task Send(EmailMessage message)
        {
            message.SetBodyFromTemplate(isTest);
            var mm = new MimeMessage();
            mm.From.Add(new MailboxAddress(message.From));
            mm.To.Add(new MailboxAddress(message.To));
            mm.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Body;
            mm.Body = bodyBuilder.ToMessageBody();

            client.Connect(smtpUrl, 465, true);
            client.Authenticate(smtpUsername, smtpPassword);
            await client.SendAsync(mm);
            client.Disconnect(true);
        }
    }
}
