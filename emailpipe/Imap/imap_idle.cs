//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using MailKit.Net.Imap;
using System;
using System.Text;
using System.Threading;

namespace emailpipe
{
    public class ImapIdle
    {
        private readonly ImapClient _imapClient;
        private readonly MailManager _mailManager;
        private StringBuilder _statusText;
        public ImapIdle(ImapClient imapClient, MailManager mailManager, StringBuilder statusText)
        {
            _imapClient = imapClient;
            _mailManager = mailManager;
            _statusText = statusText;
        }
        
        //TODO IF Idle is not supported we should maybe send Noop command... then it could act almost like Idle.
        public async void Listen()
        {
            try
            {
                //Subscribe on events on our Inbox!
                _imapClient.Inbox.CountChanged += Inbox_CountChanged;

                await _imapClient.IdleAsync(new CancellationTokenSource().Token); //TODO Make better use of CancelleationToken
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (ArgumentException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //Some Weird issue happen maybe missing cancellation token or something :(
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (ObjectDisposedException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //Should not be disposed thought.
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (OperationCanceledException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //Operation was cancelled with Call
            }
        }

        private void Inbox_CountChanged(object sender, EventArgs e)
        {
            //TODO ADD SOME KIND OF METHOD TO QUE UP MAIL BECAUSE IT WILL MOST PROBOBLY FAIL TO MAKE 2098945 millions connection at the same time.
            _mailManager.NewMessageSignal(_imapClient, _statusText);
        }

        private void Inbox_MessagesArrived(object sender, MailKit.MessageEventArgs e)
        {
            //TODO ADD SOME KIND OF METHOD TO QUE UP MAIL BECAUSE IT WILL MOST PROBOBLY FAIL TO MAKE 2098945 millions connection at the same time.
            //_mailManager.NewMessageSignal(_imapClient, _statusText);
        }
    }
}
