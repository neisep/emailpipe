using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe.ApiRepo
{
    public abstract class ApiRepoBase : IApiRepo
    {
        public abstract string ApiPath { get; set; }
        public abstract string ApiKey1 { get; set; }
        public abstract string ApiKey2 { get; set; }
        public abstract string ApiKey3 { get; set; }
        public abstract string ApiKey4 { get; set; }

        public abstract void AddnewTicket(MailMessage eml);
        public abstract void CloseTicket(MailMessage eml);
        public abstract void UpdateTicket(MailMessage eml);
    }
}
