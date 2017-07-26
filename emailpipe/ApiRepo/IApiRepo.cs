using System.Net.Mail;

namespace emailpipe.ApiRepo
{
    public interface IApiRepo
    {
        void AddnewTicket(MailMessage eml);
        void CloseTicket(MailMessage eml);
        void UpdateTicket(MailMessage eml);
    }
}
