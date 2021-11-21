using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Recruiter.Domain.Model;
using Microsoft.Extensions.Configuration;
using Recruiter.API.Services;
using Recruiter.API.Common.Constants;

namespace Recruiter.API.Service
{
    public class AuthenService : IAuthenService
    {
        public TokenInfo RequestToken(User user, IConfiguration configuration)
        {
            try
            {
                var claims = new[]
                {
                   new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                   new Claim(CustomClaimTypes.Username, user.UserName.Trim()),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(CustomClaimTypes.Email, user.Email.Trim())
                };
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityToken:Key"]));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: configuration["JwtSecurityToken:Issuer"],
                    audience: configuration["JwtSecurityToken:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signingCredentials
                );
                return new TokenInfo
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Expiration = jwtSecurityToken.ValidTo
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
