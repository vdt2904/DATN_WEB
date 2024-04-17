using DATNWEB.helpter;

namespace DATNWEB.Service
{
    public interface IMailService
    {
        Task SendEmailAsync (MailRequest mailrequest);
    }
}
