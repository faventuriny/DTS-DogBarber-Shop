using DTS_DogBarber_Shop.Data.Models.Dtos.Requests;
using DTS_DogBarber_Shop.Data.Models.Dtos.Responses;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DTS_DogBarber_Shop.Auth
{
    public interface IAuthRepo
    {
        Task<RegistrationResponse> Register(UserRegistrationDto user);
        Task<RegistrationResponse> Login(UserLoginRequest user);
        string GenerateJwtToken(IdentityUser user);
    }
}
