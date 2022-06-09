using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo.Core.Dtos;
using Todo.Core.Settings;

namespace Todo.DAL.Interfaces
{
    public class TokenizationSetting : ITokenizationSetting
    {
        private readonly JwtSettings jwtSettings;

        public TokenizationSetting(IOptions<JwtSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }

        public string GenerateJwtToken(TokenDataDto tokenData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.AuthKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", tokenData.UserId.ToString()),
                    new Claim("email", tokenData.Email),
                    new Claim("mobileNumber", tokenData.MobileNumber),
                    new Claim("applicationUserName", tokenData.Name),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
