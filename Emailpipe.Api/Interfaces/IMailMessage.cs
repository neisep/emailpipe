//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emailpipe.Api.Interfaces
{
    public interface IMailMessage
    {
        IMessageInfo From { get; set; }
        // ICollection<IMessageInfo> Attachments { get; set; } //TODO FIX LATER
        // ICollection<IMessageInfo> BodyParts { get; set; } //TODO FIX LATER

        string Subject { get; set; }
        DateTimeOffset Date { get; set; }

        string TextBody { get; set; }

        /// <summary>
        /// Convert our mail to jsonString
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        string ConvertMailToJson(IMailMessage message);
    }

    public interface IMessageInfo
    {
        string Name { get; set; }
        ICollection<string> Mailboxes { get; set; }
    }
}
