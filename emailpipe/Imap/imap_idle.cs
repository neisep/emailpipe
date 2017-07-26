using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace emailpipe
{
    public class Imap_idle
    {
        private ImapClient _imapClient;
        private MailManager _mailManager;
        public Imap_idle(ImapClient imapClient, MailManager mailManager)
        {
            _imapClient = imapClient;
            _mailManager = mailManager;
        }
        
        //TODO IF Idle is not supported we should maybe send Noop command... then it could act almost like Idle.
        public async void Listen()
        {
            try
            {
                //Subscribe on events on our Inbox!
                _imapClient.Inbox.MessagesArrived += Inbox_MessagesArrived;

                await _imapClient.IdleAsync(new CancellationTokenSource().Token); //TODO Make better use of CancelleationToken
            }
            catch (ArgumentException ex)
            {
                //Some Weird issue happen maybe missing cancellation token or something :(
            }
            catch (ObjectDisposedException ex)
            {
                //Should not be disposed thought.
            }
            catch (OperationCanceledException ex)
            {
                //Operation was cancelled with Call
            }
        }

        private void Inbox_MessagesArrived(object sender, MailKit.MessagesArrivedEventArgs e)
        {
            //TODO ADD SOME KIND OF METHOD TO QUE UP MAIL BECAUSE IT WILL MOST PROBOBLY FAIL TO MAKE 2098945 millions connection at the same time.
            _mailManager.NewMessageSignal();
        }
    }
}
