using DTS_DogBarber_Shop.Auth.Configuration;
using DTS_DogBarber_Shop.Data.Models.Dtos.Requests;
using DTS_DogBarber_Shop.Data.Models.Dtos.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DTS_DogBarber_Shop.Auth
{
    public class AuthService : IAuthRepo
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        public async Task<RegistrationResponse> Register(UserRegistrationDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Email already in use"
                            },
                    Success = false
                };
            };
            var newUser = new IdentityUser() { Email = user.Email, UserName = user.UserName };
            var isCreated = await _userManager.CreateAsync(newUser, user.Password);
            if (isCreated.Succeeded)
            {
                var jwtToken = GenerateJwtToken(newUser);

                return new RegistrationResponse()
                {
                    Success = true,
                    Token = jwtToken,
                    UserId = newUser.Id,
                    UserName = newUser.UserName

                };
            }
            else
            {
                return new RegistrationResponse()
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                };
            }
        }

        public async Task<RegistrationResponse> Login(UserLoginRequest user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Invalid login request"
                            },
                    Success = false
                };
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if (!isCorrect)
            {
                return new RegistrationResponse()
                {
                    Errors = new List<string>() {
                                "Invalid login request"
                            },
                    Success = false
                };
            }
            
            var jwtToken = GenerateJwtToken(existingUser);

            return new RegistrationResponse()
            {
                Success = true,
                Token = jwtToken,
                UserId = existingUser.Id,
                UserName = existingUser.UserName
            };
        }

        public string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}

