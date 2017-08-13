using MimeKit;

namespace emailpipe.ApiRepo
{
    public interface IApiRepo
    {
        void AddnewTicket(MimeMessage eml);
        void CloseTicket(MimeMessage eml);
        void UpdateTicket(MimeMessage eml);
    }
}
