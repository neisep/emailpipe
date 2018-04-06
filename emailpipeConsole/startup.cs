using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emailpipe.Common;
using Emailpipe.Common.Models;
using Newtonsoft.Json;

namespace emailpipeConsole
{
    public class Startup
    {
        private string _jsonSettings;
        public Settings Settings { get; private set; }
        private byte[] _key;

        public void Load()
        {
            CreateKeyFile();
            LoadKeyFile();
            LoadSettings();
        }

        public void Execute()
        {
            //Execute something fun here!
            //Maybe run code for Imap
            if (Settings == null)
            {
                Console.WriteLine("Could not start application, missing configuration");
                return;
            }



        }

        /// <summary>
        /// Creates CryptoKey
        /// </summary>
        private void CreateKeyFile()
        {
            if (!File.Exists("ashibashi.nei"))
                File.WriteAllBytes("ashibashi.nei", AESGCM.NewKey());
        }

        /// <summary>
        /// Loads Crypto key
        /// </summary>
        private void LoadKeyFile()
        {
            if (File.Exists("ashibashi.nei"))
                _key = File.ReadAllBytes("ashibashi.nei");
        }

        /// <summary>
        /// Loads settings and keeps it in memory
        /// </summary>
        private void LoadSettings()
        {
            if (File.Exists("settings.json"))
                _jsonSettings = File.ReadAllText("settings.json");

            if(!string.IsNullOrEmpty(_jsonSettings))
                Settings = JsonConvert.DeserializeObject<Settings>(_jsonSettings);

            _jsonSettings = null;
        }

    }
}
