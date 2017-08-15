using MimeKit;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace emailpipe.ApiRepo
{
    public class katak : ApiRepoBase
    {
        public override string ApiKey1 { get; set; }

        public override string ApiKey2 { get; set; }

        public override string ApiKey3 { get; set; }

        public override string ApiKey4 { get; set; }

        public override string ApiAdress { get; set; }

        public override void AddnewTicket(MimeMessage eml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiAdress);
                request.UserAgent = ApiKey1;
                request.Method = "POST";
                request.ContentType = "text/plain; charset=UTF-8;";

                byte[] data = Encoding.UTF8.GetBytes(eml.ToString());

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch
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
    }
}
