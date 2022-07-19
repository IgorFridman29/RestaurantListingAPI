using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestaurantListingAPI.Data;
using RestaurantListingAPI.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantListingAPI.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;



        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        //this method makes sure the user is registered
        public async Task<bool> ValidateUser(LoginUserDTO userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_user, userDTO.Password);
            return (_user != null && validPassword);
        }

        public async Task<string> CreateToken()
        {
            // get the key and algorithm
            var signingCredentials = GetSigningCredentials();
            // get the user claims to be added to the token body
            var claims = await GetClaims();

            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            //to create the token we need to add the ket and to decide on hashing algorithm
            // get the key from the system environment variable
            var key = _configuration.GetSection("Jwt").GetSection("Key").Value;
            // encode it as a byte array that's why we use UTF8
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            //generate the token
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims()
        {
            //The user claim to be someone and have priviliges to do something
            /*
             * For example if you want access to a night club the authorization process might be:
             * The door security officer would evaluate the value of your date of birth claim 
             * and whether they trust the issuer (the driving license authority) before granting you access.
             */

            // 
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, _user.UserName),
                 new Claim(ClaimTypes.MobilePhone, _user.PhoneNumber),
                 new Claim("IsPaid", _user.IsPaid.ToString())

             };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var issuer = jwtSettings.GetSection("Issuer").Value;
            var audience = jwtSettings.GetSection("Audience").Value;
            var lifeTime = jwtSettings.GetSection("Lifetime").Value;
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(lifeTime));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }
    }
}
