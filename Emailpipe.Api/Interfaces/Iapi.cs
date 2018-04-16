//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using Emailpipe.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emailpipe.Api.Interfaces
{
    public interface Iapi
    {
        string ApiKey1 { get; set; }
        string ApiKey2 { get; set; }
        string ApiAdress { get; set; }
        void AddnewTicket(IMailMessage message);
        void CloseTicket(IMailMessage message);
        void UpdateTicket(IMailMessage message);
    }
}
