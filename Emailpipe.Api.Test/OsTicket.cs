//
// Copyright (c) 2018 Jimmie Jönsson <jimmie@neisep.com>
//
using System;
using System.IO;
using Emailpipe.Common.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Emailpipe.Common;
using MimeKit;
using System.Collections.Generic;

namespace Emailpipe.Api.Test
{
    [TestClass]
    public class UnitTestOsTicket
    {
        public Settings LoadSettings()
        {
            var domainPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\"));
            var settingsPath = Path.GetFullPath(Path.Combine(domainPath, "emailpipe\\bin\\Debug"));
            var settingsFileLocation = string.Format("{0}\\settings.json", settingsPath);

            if (!File.Exists(settingsFileLocation))
                return null;

            var textFile = File.ReadAllText(settingsFileLocation);
            var json = JsonConvert.DeserializeObject<Settings>(textFile);

            return json;
        }

        public MimeMessage CreateDefaultMessage()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Jimmie", "jimmie@neisep.com"));
            message.To.Add(new MailboxAddress("Support", "support@neisep.com"));
            message.Subject = "Hello i need help plz";

            message.Body = new TextPart("plain")
            {
                Text = @"Hello Support,
                My computer wont start and i dont know what the problem is
                There is some weird noice coming from the harddrive it sounds like a knocking..                

                -- Jimmie
                "
            };

            return message;
        }

        [TestMethod]
        public void PostEmailWithoutAttachmentToApi()
        {
            var settings = LoadSettings();
            if (settings == null)
                throw new IOException("Settings was never set, file maybe was not found");

            var api = new OsTicket
            {
                ApiAdress = settings.ApiAdress,
                ApiKey = settings.ApiKey,
            };

            var mimeMessage = CreateDefaultMessage();

            var message = new MailMessage(mimeMessage);

            api.AddnewTicket(message);
        }

        [TestMethod]
        public void PostEmailWithAttachmentToApi()
        {
            //TODO ADD ATTACHMENT MAIL HERE!
        }
    }
}
