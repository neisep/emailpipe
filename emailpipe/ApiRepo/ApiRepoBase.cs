using MimeKit;

namespace emailpipe.ApiRepo
{
    public abstract class ApiRepoBase : IApiRepo
    {
        public abstract string ApiAdress { get; set; }
        public abstract string ApiKey1 { get; set; }
        public abstract string ApiKey2 { get; set; }

        public abstract void AddnewTicket(MimeMessage eml);
        public abstract void CloseTicket(MimeMessage eml);
        public abstract void UpdateTicket(MimeMessage eml);
    }
}
