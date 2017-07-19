using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe.ApiRepo
{
    public interface IApiRepo
    {
        void AddnewTicket(MailMessage eml);
        void CloseTicket(MailMessage eml);
        void UpdateTicket(MailMessage eml);
    }
}
