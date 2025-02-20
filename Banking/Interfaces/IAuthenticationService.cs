using Banking.Models.DTOs;

namespace Banking.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task RegisterAsync(RegisterDto registerDto);
    }
}
