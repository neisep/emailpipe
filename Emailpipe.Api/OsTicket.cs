using Emailpipe.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Emailpipe.Api
{
    public class OsTicket : Iapi
    {
        public string ApiKey1 { get; set; }
        public string ApiKey2 { get; set; }
        public string ApiAdress { get; set; }

        public ICollection<IMailMessage> FailedMessages { get; set; }

        public void AddnewTicket(IMailMessage message)
        {
            try
            {
                using (var client = new WebClient() { Encoding = System.Text.Encoding.UTF8 })
                {
                    client.Headers.Add("X-API-Key", ApiKey1);
                    client.Headers.Add("Expect", string.Empty);
                    client.Headers.Add("User-Agent", "emailPipe");

                    client.UploadString(ApiAdress, "POST", message.ConvertMailToJson(message));
                }
            }
            catch (Exception ex)
            {
                //TODO ADD SOME KIND OF ERROR HANDLING!
                FailedMessages.Add(message);
            }
        }

        public void CloseTicket(IMailMessage message)
        {
            throw new NotImplementedException();
        }

        public void UpdateTicket(IMailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
