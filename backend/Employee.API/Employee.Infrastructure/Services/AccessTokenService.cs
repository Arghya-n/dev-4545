﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Employee.Infrastructure.Services
{
    public class AccessTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _dbContext;

        public AccessTokenService(IConfiguration configuration, AppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<JwtSecurityToken> GenerateToken(EmployeeEntity employee)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            // NOSONAR - This key is loaded securely from non-published config
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.Name),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Permissions), employee.Role)),
                new Claim("EmployeeId", employee.EmployeeId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return token;
        }
    }
}
