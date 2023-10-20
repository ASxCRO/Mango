using Mango.Service.AuthAPI.Models.Dto;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage = await _authService.Register(registrationRequestDto);

            var result = new ResponseDto<object>();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                result.IsSuccess = false;
                result.Message = errorMessage;
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);

            var result = new ResponseDto<object>();
            if (loginResponse.User == null)
            {
                result.IsSuccess = false;
                result.Message = "User or password incorrect";
                return BadRequest(result);
            }

            result.Result = loginResponse;
            return Ok(result);
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccess = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper()) ;

            var result = new ResponseDto<object>();
            if (!assignRoleSuccess)
            {
                result.IsSuccess = false;
                result.Message = "Error encountered";
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
