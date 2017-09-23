using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace emailpipe.ApiRepo
{
    class OsTicket : ApiRepoBase
    {
        public override string ApiKey1 { get; set; }

        public override string ApiKey2 { get; set; }

        public override string ApiAdress { get; set; }

        public override void AddnewTicket(MimeMessage eml)
        {
            try
            {
                using (var client = new WebClient() {Encoding = System.Text.Encoding.UTF8})
                {
                    client.Headers.Add("X-API-Key", ApiKey1);
                    client.Headers.Add("Expect", string.Empty);
                    client.Headers.Add("User-Agent", "emailPipe");

                    client.UploadString(ApiAdress, "POST", CreateTicketJSON(eml));
                }
            }
            catch(Exception ex)
            {
                //TODO ADD SOME KIND OF ERROR HANDLING!
            }
        }

        public override void CloseTicket(MimeMessage eml)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTicket(MimeMessage eml)
        {
            throw new NotImplementedException();
        }

        private string CreateTicketJSON(MimeMessage eml)
        {
            var jsonList = new Dictionary<string, object>();

            var name = eml.From.First().Name;
            if (string.IsNullOrEmpty(eml.From.First().Name))
            {
                name = eml.From.First().ToString();
                name = name.Substring(0, name.IndexOf('@'));
            }

            jsonList.Add("name", name);
            jsonList.Add("email", eml.From.Mailboxes.First().Address);
            jsonList.Add("source", "API");
            jsonList.Add("subject", eml.Subject);
            jsonList.Add("message", eml.TextBody.Trim());

            //TODO INCLUDE ATTACHMENT!

            var jsonString = JsonConvert.SerializeObject(jsonList);
            return jsonString;
        }
    }
}
