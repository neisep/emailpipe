﻿//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using Emailpipe.Api.Interfaces;

namespace Emailpipe.Api
{
    public class Base : Iapi
    {
        public string ApiKey { get; set; }
        public string ApiKey2 { get; set; }
        public string ApiAdress { get; set; }

        public void AddnewTicket(IMailMessage message)
        {
        }

        public void CloseTicket(IMailMessage message)
        {
        }

        public void UpdateTicket(IMailMessage message)
        {
        }
    }
}
