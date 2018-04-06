using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emailpipe.Api.Interfaces;
using MimeKit;
using Newtonsoft.Json;

namespace emailpipe
{
    public class MailMessage : IMailMessage
    {
        public IMessageInfo From { get; set; }

        public string Subject { get; set; }
        public DateTimeOffset Date { get; set; }
        public string TextBody { get; set; }

        //TODO UNCOMMENTED FOR NOW NEEDS TO BE FIXED
        public MailMessage(MimeMessage message)
        {
            //From = new MessageInfo(message);
            //foreach (var item in message.From)
            //{
            //    From.Add(new MessageInfo(item));
            //}
        }

        public void GetMessage()
        {
            //TODO Write something fancy here maybe?
        }

        //TODO UNCOMMENTED FOR NOW NEEDS TO BE FIXED
        public string ConvertMailToJson(MailMessage message)
        {
            //var jsonList = new Dictionary<string, object>();

            //var name = message.From.First().Name;
            //if (string.IsNullOrEmpty(message.From.First().Name))
            //{
            //    name = message.From.First().ToString();
            //    name = name.Substring(0, name.IndexOf('@'));
            //}

            //jsonList.Add("name", name);
            //jsonList.Add("email", message.From.First().Mailboxes);
            //jsonList.Add("source", "API");
            //jsonList.Add("subject", message.Subject);
            //jsonList.Add("message", message.TextBody.Trim());

            ////TODO INCLUDE ATTACHMENT!

            //var jsonString = JsonConvert.SerializeObject(jsonList);
            //return jsonString;
            return null;
        }

        public string ConvertMailToJson(IMailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
