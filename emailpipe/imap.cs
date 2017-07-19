using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using S22.Imap;
using System.Net.Mail;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace emailpipe
{
    public class imap
    {
        private ObservableCollection<ListViewItem> _emailListViewItem;
        private string _ip;
        private int _port;
        private string _username;
        private string _password;
        public imap(string ip, int port, string username, string password, ObservableCollection<ListViewItem> emailListViewItem)
        {
            _ip = ip;
            _port = port;
            _username = username;
            _password = password;
            _emailListViewItem = emailListViewItem;
        }

        public void Listen()
        {
            Task test = new Task(() =>
            {
                try
                {
                    using (ImapClient imapClient = new ImapClient(_ip, _port, _username, _password))
                    {
                        if (!imapClient.Supports("IDLE"))
                        {
                            MessageBox.Show("Mail server does not support IDLE command");
                            return;
                        }

                        imapClient.NewMessage += new EventHandler<IdleMessageEventArgs>(ImapClient_NewMessage);

                        while (true)
                        {
                            System.Threading.Thread.Sleep(6000);
                        }
                    }
                }
                catch (Exception crap)
                {
                    //TODO FIX SOME ERROR CODE OR SOMETHING HERE!
                    //Listen();
                }

            });

            test.Start();
        }

        private void ImapClient_NewMessage(object sender, IdleMessageEventArgs e)
        {
            MailMessage eml = e.Client.GetMessage(e.MessageUID, FetchOptions.Normal);
            var header = eml.Headers;

            StringBuilder messageString = new StringBuilder();

            //Puts together the header.
            for (int i = 0; i < header.Count; i++)
            {
                messageString.Append("" + header.Keys[i] + ": " + header[i].ToString() + "");
                messageString.AppendLine();
            }

            messageString.AppendLine();
            messageString.Append(eml.Body);

            var data = Encoding.UTF8.GetBytes(messageString.ToString());

            var runningDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            runningDir = runningDir + "\\emails";

            if (!System.IO.Directory.Exists(runningDir))
                System.IO.Directory.CreateDirectory(runningDir);

            string fileName = string.Format("{1}\\email_{0}.eml", e.MessageUID, runningDir);
            File.WriteAllBytes(fileName, data);

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListViewItem listViewItem = new ListViewItem();

                string emailDate = DateTime.Now.ToString();
                if (eml.Date() != null)
                    emailDate = ((DateTime)eml.Date()).ToString("yyyy-MM-dd HH:mm:ss");

                listViewItem.Content = new EmailListViewItem { Subject = eml.Subject, Date = emailDate };
                listViewItem.Tag = eml;
                _emailListViewItem.Add(listViewItem);

            });
        }
    }

    public class EmailListViewItem
    {
        public string Subject { get; set; }
        public string Date { get; set; }
    }
}
