//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using MimeKit;

namespace emailpipe.DatabaseRepo
{
    public interface IDatabase
    {
        void AddMail(MimeMessage eml);
        void RemoveMail(MimeMessage eml);
        bool CheckIfMailExists(long ident);
        void Dispose();
    }
}
