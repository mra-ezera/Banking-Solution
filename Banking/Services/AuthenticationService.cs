using Banking.Data;
using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace Banking.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _dbContext.LoginUsers.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto), "RegisterDto cannot be null");
            }

            if (string.IsNullOrEmpty(registerDto.Username))
            {
                throw new ArgumentNullException(nameof(registerDto.Username), "Username cannot be null or empty");
            }

            if (string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentNullException(nameof(registerDto.Password), "Password cannot be null or empty");
            }

            var user = new LoginUser
            {
                Id = Guid.NewGuid(),
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };
            _dbContext.LoginUsers.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
