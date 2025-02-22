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

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.NotNull(result.Value);

            var tokenResult = result.Value as TokenResult;
            Assert.NotNull(tokenResult);
            Assert.Equal("test_token", tokenResult.Token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            var loginDto = new LoginDto { Username = "testuser", Password = "wrongpassword" };
            _authenticationServiceMock.Setup(service => service.LoginAsync(loginDto))
                .ThrowsAsync(new UnauthorizedAccessException());

            var result = await _authenticationController.Login(loginDto) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);

            var errorResponse = Assert.IsType<ErrorResponse>(result.Value);
            Assert.Equal("Invalid credentials", errorResponse.Error);
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
            var result = await _authenticationController.Register(null) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);

            var errorResponse = Assert.IsType<ErrorResponse>(result.Value);
            Assert.Equal("Register data is required", errorResponse.Error);
        }

        [Fact]
        public async Task Register_ThrowsException_ReturnsInternalServerError()
        {
            var registerDto = new RegisterDto { Username = "newuser", Password = "password" };
            _authenticationServiceMock.Setup(service => service.RegisterAsync(registerDto))
                .ThrowsAsync(new Exception());

            var result = await _authenticationController.Register(registerDto) as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

            var errorResponse = Assert.IsType<ErrorResponse>(result.Value);
            Assert.Equal("An error occurred while processing your request", errorResponse.Error);
        }
    }
}
