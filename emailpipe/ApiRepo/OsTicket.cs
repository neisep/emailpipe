using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe.ApiRepo
{
    class OsTicket : ApiRepoBase
    {
        public override string ApiKey1 { get; set; }

        public override string ApiKey2 { get; set; }

        public override string ApiKey3 { get; set; }

        public override string ApiKey4 { get; set; }

        public override string ApiPath { get; set; }

        public override void AddnewTicket(MailMessage eml)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Expect", string.Empty);
                client.Headers.Add("X-API-Key", ApiKey1);

                client.UploadString(ApiPath, "POST", CreateTicketJSON(eml));
            }
            catch(Exception crap)
            {
                //TODO ADD SOME KIND OF ERROR HANDLING!
            }
        }

        public override void CloseTicket(MailMessage eml)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTicket(MailMessage eml)
        {
            throw new NotImplementedException();
        }

        private string CreateTicketJSON(MailMessage eml)
        {
            Dictionary<string, object> jsonList = new Dictionary<string, object>();
            jsonList.Add("name", eml.From.User);
            jsonList.Add("email", eml.From.Address);
            jsonList.Add("source", "API");
            jsonList.Add("subject", eml.Subject);
            jsonList.Add("message", eml.Body); //ÅÄÖ NOT REALLY WORKING RIGHT NOW :(

            //TODO ADD ATTACHMENT!

            var jsonString = JsonConvert.SerializeObject(jsonList);
            return jsonString;
        }
    }
}
