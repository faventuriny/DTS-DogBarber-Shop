using System.ComponentModel.DataAnnotations;


namespace DTS_DogBarber_Shop.Data.Models.Dtos.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
