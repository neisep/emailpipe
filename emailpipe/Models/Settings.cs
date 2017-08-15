using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emailpipe.Models
{
    public class Settings
    {
        public string ApiAdress { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //TODO ADD ENCRYPTION
        public string EmailServerAdress { get; set; }
    }
}
