using System.Collections.Generic;
using Emailpipe.Api.Interfaces;
using MimeKit;

namespace emailpipe
{
    public class MessageInfo : IMessageInfo
    {
        public string Name { get; set; }
        public ICollection<string> Mailboxes { get; set; }

        public MessageInfo(InternetAddress message)
        {
            Name = message.Name;
        }
    }
}
