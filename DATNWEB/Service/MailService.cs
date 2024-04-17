using DATNWEB.helpter;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DATNWEB.Service
{
    public class MailService : IMailService
    {
        private readonly MailSetting mailSetting;
        public MailService(IOptions<MailSetting> options)
        {
            this.mailSetting = options.Value;
        }
        public async Task SendEmailAsync(MailRequest mailrequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(mailSetting.Displayname,mailSetting.Email));
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.Subject = mailrequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailrequest.Body;
            email.Body=builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(mailSetting.Host, mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(mailSetting.Email, mailSetting.Pass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
