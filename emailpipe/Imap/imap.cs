using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MailKit.Net.Imap;
using MailKit;
using System.Threading;
using System.Linq;

namespace emailpipe
{
    public class imap
    {
        private ObservableCollection<IMessageSummary> _observableCollectionEmails;

        private ObservableCollection<ListViewItem> _emailListViewItem;
        private string _ip;
        private int _port;
        private string _username;
        private string _password;
        private MailManager _mailManager;
        public imap(string ip, int port, string username, string password, ObservableCollection<ListViewItem> emailListViewItem)
        {
            _ip = ip;
            _port = port;
            _username = username;
            _password = password;
            _emailListViewItem = emailListViewItem;
            _observableCollectionEmails = new ObservableCollection<IMessageSummary>();
            _observableCollectionEmails.CollectionChanged += _observableCollectionEmails_CollectionChanged;
        }

        public void Listen()
        {
            _mailManager = new MailManager(_observableCollectionEmails);

            Task imapFetchTask = ImapTask(ImapType.Fetch, _mailManager);
            Task imapIdleTask = ImapTask(ImapType.Idle, _mailManager);

            imapIdleTask.Start();
            imapFetchTask.Start();
        }

        private Task ImapTask(ImapType imapType, MailManager mailManager)
        {
            return new Task(() =>
            {
                try
                {
                    using (ImapClient imapClient = new ImapClient(new ProtocolLogger(Console.OpenStandardError())))
                    {
                        // Remove the XOAUTH2 authentication mechanism since we don't have an OAuth2 token.
                        imapClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        imapClient.Connect(_ip, _port, MailKit.Security.SecureSocketOptions.None);
                        imapClient.Authenticate(_username, _password);

                        imapClient.Inbox.Open(FolderAccess.ReadOnly);

                        if (!imapClient.Capabilities.HasFlag(ImapCapabilities.Idle))
                        {
                            MessageBox.Show("Mail server does not support IDLE command");
                            return;
                        }

                        if (imapType == ImapType.Fetch)
                        {
                            Imap_fetch imapFetch = new Imap_fetch(imapClient);
                            mailManager.AddMailToCollection(imapFetch.ReceiveMails());
                        }
                        else if(imapType == ImapType.Idle)
                        {
                            Imap_idle imapIdle = new Imap_idle(imapClient, _mailManager);
                            imapIdle.Listen();

                            while(imapClient.IsConnected)
                            {
                                Thread.Sleep(100);
                            }
                        }
                    }
                }
                catch (ServiceNotConnectedException ex)
                {
                    //ImapClient not connected
                }
                catch (ServiceNotAuthenticatedException ex)
                {
                    //ImapClient not Authenticated :(
                }
                catch (InvalidOperationException ex)
                {
                    //May happen if the imapFolder was not opened
                }
                catch (NotSupportedException ex)
                {
                    //Not so good since we kind of need it ;(
                }
                catch (IOException ex)
                {
                }
                catch (ImapCommandException ex)
                {
                }
                catch (ImapProtocolException ex)
                {
                }
                catch (Exception ex)
                {
                }
            });
        }

        //private void ImapClient_NewMessage(object sender, IdleMessageEventArgs e)
        //{
        //    MailMessage eml = e.Client.GetMessage(e.MessageUID, FetchOptions.Normal);
        //    var header = eml.Headers;

        //    StringBuilder messageString = new StringBuilder();

        //    //Puts together the header.
        //    for (int i = 0; i < header.Count; i++)
        //    {
        //        messageString.Append("" + header.Keys[i] + ": " + header[i].ToString() + "");
        //        messageString.AppendLine();
        //    }

        //    messageString.AppendLine();
        //    messageString.Append(eml.Body);

        //    var data = Encoding.ASCII.GetBytes(messageString.ToString());

        //    var runningDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        //    runningDir = runningDir + "\\emails";

        //    if (!System.IO.Directory.Exists(runningDir))
        //        System.IO.Directory.CreateDirectory(runningDir);

        //    string fileName = string.Format("{1}\\email_{0}.eml", e.MessageUID, runningDir);
        //    File.WriteAllBytes(fileName, data);

        //    Application.Current.Dispatcher.Invoke((Action)delegate
        //    {
        //        ListViewItem listViewItem = new ListViewItem();

        //        string emailDate = DateTime.Now.ToString();
        //        if (eml.Date() != null)
        //            emailDate = ((DateTime)eml.Date()).ToString("yyyy-MM-dd HH:mm:ss");

        //        listViewItem.Content = new EmailListViewItem { Subject = eml.Subject, Date = emailDate };
        //        listViewItem.Tag = eml;
        //        _emailListViewItem.Add(listViewItem);

        //    });
        //}

       /// <summary>
       /// If we receive any new email it will be added here.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void _observableCollectionEmails_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
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

    }

    public class EmailListViewItem
    {
        public string Subject { get; set; }
        public string Date { get; set; }
    }

    public enum ImapType
    {
        Fetch = 0,
        Idle = 1,
    }
}
