using Banking.Interfaces;
using Banking.Models.DTOs;
using Banking.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Banking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        [SwaggerOperation(Summary = "Logs in a user and returns a JWT token.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _authenticationService.LoginAsync(loginDto);
                return new ContentResult
                {
                    Content = token,
                    ContentType = "text/plain",
                    StatusCode = StatusCodes.Status200OK
                };

            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ErrorResponse { Error = "Invalid credentials" });
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Registers a new user.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto? registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest(new ErrorResponse { Error = "Register data is required" });
            }

            if (await _authenticationService.UsernameExistsAsync(registerDto.Username))
            {
                return BadRequest(new ErrorResponse { Error = "Username already exists" });
            }

            try
            {
                await _authenticationService.RegisterAsync(registerDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, new ErrorResponse { Error = "An error occurred while processing your request" });
            }
        }
    }
}
