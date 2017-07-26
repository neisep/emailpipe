using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe
{
    public class Imap_fetch
    {
        private ImapClient _imapClient;
        public Imap_fetch(ImapClient imapClient)
        {
            _imapClient = imapClient;
        }

        public List<IMessageSummary> ReceiveMails()
        {
            List<IMessageSummary> mailList = _imapClient.Inbox.Fetch(0, -1, MessageSummaryItems.Full | MessageSummaryItems.UniqueId).ToList();

            _imapClient.Disconnect(true);

            return mailList;
        }
    }
}
