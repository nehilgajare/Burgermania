using Azure.Core;
using Burgermania.Models;
using Burgermania.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Burgermania.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GetToken(string mobileno)
        {
            var tokenNum = new String(mobileno + DateTime.Now);

            var claim = new []
            {
                new Claim(JwtRegisteredClaimNames.Sub,tokenNum),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            //A symmetric key means that the same key is used for both signing the token and verifying its signature.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claim,
                expires: DateTime.Now.AddDays(1),
                //you can implement Refresh Token
                signingCredentials:creds

                );
            //The WriteToken method takes a JwtSecurityToken object as an argument and serializes it into a compact,
            //URL-safe string format. This string is the actual JWT that will be sent to the client or used in API requests.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
