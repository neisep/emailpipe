using MailKit.Net.Imap;
using MimeKit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace emailpipe
{
    public class MailManager
    {
        private readonly Imap _imap;
        private ObservableCollection<MimeMessage> _observableCollectionEmails;
        public MailManager(ObservableCollection<MimeMessage> observableCollectionEmails, Imap imap)
        {
            _observableCollectionEmails = observableCollectionEmails;
            _imap = imap;
        }

        /// <summary>
        /// Adds new mail from Inbox with new client connection.
        /// </summary>
        /// <param name="mailList"></param>
        public void AddMailToCollection(List<MimeMessage> mailList)
        {
            foreach (var mail in mailList)
            {
                if (_observableCollectionEmails.All(i => i.MessageId != mail.MessageId))
                {
                    _observableCollectionEmails.Add(mail);
                }
            }
        }

        /// <summary>
        /// Signal from ImapIdle Client lets start task to fetch new email with new connection.
        /// </summary>
        public void NewMessageSignal(ImapClient imapClient, StringBuilder statusText)
        {
                _imap?.FetchNewMail(statusText);
        }
    }
}
