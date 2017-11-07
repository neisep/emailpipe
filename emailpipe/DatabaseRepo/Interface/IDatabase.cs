using MimeKit;

namespace emailpipe.DatabaseRepo
{
    public interface IDatabase
    {
        void AddMail(MimeMessage eml);
        void RemoveMail(MimeMessage eml);
        bool CheckIfMailExists(long ident);
        void Dispose();
    }
}
