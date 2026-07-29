using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SniffAndStay.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SniffAndStay.Infrastructure.Security
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;


        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateToken(
            Guid userId,
            string email)
        {
            var jwtSettings = _configuration
                .GetSection("Jwt");


            var key = jwtSettings["Key"]!;


            var claims = new[]
            {
            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                email)
        };


            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));


            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSettings["ExpirationMinutes"]!)),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
