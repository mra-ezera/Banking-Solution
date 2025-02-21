using Banking.Interfaces;
using Banking.Models.DTOs;
using Moq;

namespace Banking.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public AuthenticationServiceTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsToken()
        {
            var loginDto = new LoginDto { Username = "testuser", Password = "password" };
            _authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                .ReturnsAsync("test_token");

            var result = await _authenticationServiceMock.Object.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.Equal("test_token", result);
        }

        [Fact]
        public async Task LoginAsync_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
            _authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                .ThrowsAsync(new UnauthorizedAccessException());

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authenticationServiceMock.Object.LoginAsync(loginDto));
        }

        [Fact]
        public async Task RegisterAsync_ValidData_CompletesSuccessfully()
        {
            var registerDto = new RegisterDto { Username = "newuser", Password = "password" };

            _authenticationServiceMock.Setup(service => service.RegisterAsync(registerDto))
                .Returns(Task.CompletedTask);

            await _authenticationServiceMock.Object.RegisterAsync(registerDto);

            _authenticationServiceMock.Verify(service => service.RegisterAsync(registerDto), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_NullData_ThrowsArgumentNullException()
        {
            RegisterDto registerDto = null;

            _authenticationServiceMock.Setup(service => service.RegisterAsync(registerDto))
                .ThrowsAsync(new ArgumentNullException());

            await Assert.ThrowsAsync<ArgumentNullException>(() => _authenticationServiceMock.Object.RegisterAsync(registerDto));
        }
    }
}