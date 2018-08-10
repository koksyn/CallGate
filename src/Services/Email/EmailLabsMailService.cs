using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace CallGate.Services.Email
{
    public class EmailLabsMailService : IMailService
    {
        private readonly IOptions<ConfigurationManager> _configurationManager;

        public EmailLabsMailService(IOptions<ConfigurationManager> configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void SendMail(string mailTo, string subject, string body)
        {
            var client = new SmtpClient
            {
                Port = _configurationManager.Value.MailPort,
                Host = _configurationManager.Value.MailHost,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_configurationManager.Value.MailUsername, _configurationManager.Value.MailPassword)
            };
            
            client.Send(_configurationManager.Value.MailSender, mailTo, subject, body);
        }
    }
}
