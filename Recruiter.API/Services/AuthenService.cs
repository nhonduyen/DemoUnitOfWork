using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Recruiter.Core.Entities.DbModel;
using Microsoft.Extensions.Configuration;
using Recruiter.API.Services;
using Recruiter.API.Common.Constants;
using Recruiter.Core.Entities.ViewModel.Requests;
using Recruiter.Core.Entities.ViewModel;
using System.Security.Cryptography;
using System.Linq;

namespace Recruiter.API.Service
{
    public class AuthenService : IAuthenService
    {
        private readonly IConfiguration _configuration;

        public AuthenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityToken:Key"])),
                ValidateLifetime = false // we don't care about token expiration
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        public User GetUserFromPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            var user = new User();
            var claims = claimsPrincipal.Claims;

            Guid.TryParse(claims.FirstOrDefault(x => x.Type == CustomClaimTypes.UserId).Value, out Guid userId);
            user.Id = userId;
            user.UserName = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Username).Value;
            user.Email = claims.FirstOrDefault(x => x.Type == CustomClaimTypes.Email).Value;

            return user;
        }

        public TokenInfoVM RequestToken(User user)
        {
            try
            {
                var claims = new[]
                {
                   new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                   new Claim(ClaimTypes.Name, user.UserName.Trim()),
                   new Claim(CustomClaimTypes.Username, user.UserName.Trim()),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                   new Claim(CustomClaimTypes.Email, user.Email.Trim())
                };
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityToken:Key"]));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _configuration["JwtSecurityToken:Issuer"],
                    audience: _configuration["JwtSecurityToken:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signingCredentials
                );
                return new TokenInfoVM
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
