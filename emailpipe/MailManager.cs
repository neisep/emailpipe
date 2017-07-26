using MailKit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe
{
    public class MailManager
    {
        private ObservableCollection<IMessageSummary> _observableCollectionEmails;
        public MailManager(ObservableCollection<IMessageSummary> observableCollectionEmails)
        {
            _observableCollectionEmails = observableCollectionEmails;
        }

        /// <summary>
        /// Adds new mail from Inbox with new client connection.
        /// </summary>
        /// <param name="mailList"></param>
        public void AddMailToCollection(List<IMessageSummary> mailList)
        {
            foreach (IMessageSummary mail in mailList)
            {
                _observableCollectionEmails.Add(mail);
            }
        }

        /// <summary>
        /// Signal from ImapIdle Client lets start task to fetch new email with new connection.
        /// </summary>
        public void NewMessageSignal()
        {
            //TODO ADD SOME KND OF METHOD HERE THAT WILL FETCH NEW EMAILS
        }

    }
}
