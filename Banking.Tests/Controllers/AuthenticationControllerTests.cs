using Banking.Controllers;
using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Banking.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly AuthenticationController _authenticationController;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public AuthenticationControllerTests()
        {
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _authenticationController = new AuthenticationController(_authenticationServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            var loginDto = new LoginDto { Username = "testuser", Password = "password" };
            _authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                .ReturnsAsync("test_token");

            var result = await _authenticationController.Login(loginDto) as OkObjectResult;
            Console.WriteLine($"result.Value: {result?.Value}");

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var tokenResult = result.Value as TokenResult;
            Assert.NotNull(tokenResult);

            Console.WriteLine($"Token Result: {tokenResult.Token}");

            Assert.Equal("test_token", tokenResult.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
            _authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                .ThrowsAsync(new UnauthorizedAccessException());

            var result = await _authenticationController.Login(loginDto) as UnauthorizedResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task Register_ValidData_ReturnsOkResult()
        {
            var registerDto = new RegisterDto { Username = "newuser", Password = "password" };

            var result = await _authenticationController.Register(registerDto) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task Register_NullData_ReturnsBadRequestResult()
        {
            _authenticationController.ModelState.AddModelError("Username", "Required");

            var result = await _authenticationController.Register(null) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

    }

}