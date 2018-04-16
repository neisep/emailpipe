//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emailpipe.Api.Interfaces;
using MimeKit;
using Newtonsoft.Json;

namespace Emailpipe.Common
{
    public class MailMessage : IMailMessage
    {
        public object MimeMessage { get; set; }

        public MailMessage(MimeMessage message)
        {
            MimeMessage = message;
        }

        public string ConvertMailToJson()
        {
            var jsonList = new Dictionary<string, object>();

            var message = MimeMessage as MimeMessage;

            var name = message.From.First().Name;
            if (string.IsNullOrEmpty(message.From.First().Name))
            {
                name = message.From.First().ToString();
                name = name.Substring(0, name.IndexOf('@'));
            }

            jsonList.Add("name", name);
            jsonList.Add("email", message.From.Mailboxes.First().Address);
            jsonList.Add("source", "API");
            jsonList.Add("subject", message.Subject);
            jsonList.Add("message", message.TextBody.Trim());

            //TODO FIX ATTACHMENTS!
            //1. check size of all attachment if maxlimit reached use failover
            //2. Include binary here
            //3. FailOver use thirdparty image Uploader that is private or specified by user

            var jsonString = JsonConvert.SerializeObject(jsonList);
            return jsonString;
        }
    }
}
