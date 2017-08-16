
namespace emailpipe.Models
{
    public class Settings
    {
        public string ApiAdress { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //TODO ADD ENCRYPTION
        public string EmailServerAdress { get; set; }
        public ApiTypes Type { get; set; }

        public enum ApiTypes
        {
            osTicket = 0,
            katak = 1,
        }
    }
}
