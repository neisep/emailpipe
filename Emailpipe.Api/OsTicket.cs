﻿//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using Emailpipe.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Emailpipe.Api
{
    public class OsTicket : Iapi
    {
        public string ApiKey { get; set; }
        public string ApiAdress { get; set; }

        public ICollection<IMailMessage> FailedMessages { get; set; }

        public void AddnewTicket(IMailMessage message)
        {
            try
            {
                FailedMessages = new List<IMailMessage>();
                using (var client = new WebClient() { Encoding = System.Text.Encoding.UTF8 })
                {
                    client.Headers.Add("X-API-Key", ApiKey);
                    client.Headers.Add("Expect", string.Empty);
                    client.Headers.Add("User-Agent", "emailPipe");

                    client.UploadString(ApiAdress, "POST", message.ConvertMailToJson());
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
