using System.Collections.Generic;


namespace DTS_DogBarber_Shop.Auth.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
