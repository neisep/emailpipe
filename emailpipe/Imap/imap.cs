//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using System;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MailKit.Net.Imap;
using MailKit;
using System.Threading;
using System.Linq;
using System.Text;
using emailpipe.Helper;
using emailpipe.ApiRepo;
using MimeKit;
using Emailpipe.Api.Interfaces;

namespace emailpipe
{
    public class Imap
    {
        private ObservableCollection<MimeMessage> _observableCollectionEmails;

        private ObservableCollection<ListViewItem> _emailListViewItem;
        private string _ip;
        private int _port;
        private string _username;
        private string _password;
        private MailManager _mailManager;
        public Imap(string ip, int port, string username, string password, ObservableCollection<ListViewItem> emailListViewItem)
        {
            _ip = ip;
            _port = port;
            _username = username;
            _password = password;
            _emailListViewItem = emailListViewItem;
            _observableCollectionEmails = new ObservableCollection<MimeMessage>();
            _observableCollectionEmails.CollectionChanged += _observableCollectionEmails_CollectionChanged;
        }

        //TODO CHECK WHY apihelpdesk is here ???
        public void StartMailManager(Imap imap, Iapi apihelpdesk)
        {
            _mailManager = new MailManager(_observableCollectionEmails, imap);
        }

        public void Listen(StringBuilder statusText)
        {
            var imapFetchTask = ImapTask(ImapType.Fetch, _mailManager, statusText);
            var imapIdleTask = ImapTask(ImapType.Idle, _mailManager, statusText);

            imapIdleTask.Start();
            imapFetchTask.Start();
        }

        public void FetchNewMail(StringBuilder statusText)
        {
            var imapFetchTask = ImapTask(ImapType.Fetch, _mailManager, statusText);
            imapFetchTask.Start();
        }

        public Task ImapTask(ImapType imapType, MailManager mailManager, StringBuilder statusText)
        {
            return new Task(() =>
            {
                try
                {
                    using (var imapClient = new ImapClient(new ProtocolLogger(Console.OpenStandardError())))
                    {
                        // Remove the XOAUTH2 authentication mechanism since we don't have an OAuth2 token.
                        imapClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        imapClient.Connect(_ip, _port, MailKit.Security.SecureSocketOptions.None);
                        imapClient.Authenticate(_username, _password);

                        statusText.Clear();
                        statusText.Append("Status: Connected");

                        var inbox = imapClient.Inbox;
                        inbox.Open(FolderAccess.ReadOnly);

                        if (!imapClient.Capabilities.HasFlag(ImapCapabilities.Idle))
                        {
                            MessageBox.Show("Mail server does not support IDLE command");
                            return;
                        }

                        switch (imapType)
                        {
                            case ImapType.Fetch:
                            {
                                var imapFetch = new ImapFetch(imapClient, inbox);
                                mailManager.AddMailToCollection(imapFetch.ReceiveMails());
                            }
                                break;
                            case ImapType.Idle:
                            {
                                var imapIdle = new ImapIdle(imapClient, _mailManager, statusText);
                                imapIdle.Listen();

                                while (imapClient.IsConnected)
                                {
                                    Thread.Sleep(100);
                                }
                            }
                                break;
                        }
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (ServiceNotConnectedException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //ImapClient not connected
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (ServiceNotAuthenticatedException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //ImapClient not Authenticated :(
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (InvalidOperationException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //May happen if the imapFolder was not opened
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (NotSupportedException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //Not so good since we kind of need it ;(
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (IOException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (ImapCommandException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (ImapProtocolException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                }
            });
        }

       /// <summary>
       /// If we receive any new email it will be added here.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void _observableCollectionEmails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems.OfType<MimeMessage>())
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                var listViewItem = HelperListView.ConvertToListViewItem<MimeMessage>(item);
                                if (listViewItem != null)
                                    _emailListViewItem.Add(listViewItem);
                            });
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        break;
                    default:
                        break;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //TODO Handle errors here!
            }
        }

    }

    public enum ImapType
    {
        Fetch = 0,
        Idle = 1,
    }
}
