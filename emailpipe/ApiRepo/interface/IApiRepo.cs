//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using MimeKit;

namespace emailpipe.ApiRepo
{
    public interface IApiRepo
    {
        void AddnewTicket(MimeMessage eml);
        void CloseTicket(MimeMessage eml);
        void UpdateTicket(MimeMessage eml);
    }
}
