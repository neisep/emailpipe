//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using Emailpipe.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Emailpipe.Api
{
    public class Katak
    {
        public void AddNewTicket(IMailMessage message)
        {
            //TODO ADD LOGIC!
            try
            {
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(message.Credentials.ApiAdress);
                //request.UserAgent = message.Credentials.ApiKey;
                //request.Method = "POST";
                //request.ContentType = "text/plain; charset=UTF-8;";

                //byte[] data = Encoding.UTF8.GetBytes(message.ConvertMailToJson(message));
            }
            catch
            {
                //TODO ADD ERROR HANDLING!
            }
        }
    }
}
