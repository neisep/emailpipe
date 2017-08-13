using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using emailpipe.Helper;

namespace emailpipe
{
    public class ImapFetch
    {
        private readonly IMailFolder _inbox;
        private readonly ImapClient _imapClient;
        public ImapFetch(ImapClient imapClient, IMailFolder inbox)
        {
            _imapClient = imapClient;
            _inbox = inbox;
        }

        public List<MimeMessage> ReceiveMails()
        {
            var mailList = new List<MimeMessage>();
            for (var i = 0; i < _inbox.Count; i++)
            {
                mailList.Add(_inbox.GetMessage(i));
            }

            //TODO IMPLENT SOMETHING SO WE DO NOT IMPORT SAME EMAIL AGAIN AND AGAIN!

            _imapClient.Disconnect(true);

            return mailList;
        }
    }
}
