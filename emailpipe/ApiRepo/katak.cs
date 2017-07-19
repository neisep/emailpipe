﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace emailpipe.ApiRepo
{
    public class katak : ApiRepoBase
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiPath);
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

        public override void CloseTicket(MailMessage eml)
        {
            throw new NotImplementedException();
        }

        public override void UpdateTicket(MailMessage eml)
        {
            throw new NotImplementedException();
        }
    }
}