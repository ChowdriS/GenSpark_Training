using System.Security.Claims;
using System.Threading.Tasks;
using EventBookingApi.Interface;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controller
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginRequestDTO dto)
        {
            try
            {
                var data = await _authenticationService.Login(dto);
                return Ok(ApiResponse<object>.SuccessResponse("Token succesfully generated", data));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Token generation is failed", new { ex.Message }));
            }
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO dto)
        {
            try
            {
                var result = await _authenticationService.RefreshToken(dto.RefreshToken??"");
                return Ok(ApiResponse<object>.SuccessResponse("Token refreshed", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Refresh failed", new { ex.Message }));
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (email == null || email == "") return Unauthorized();

                var result = await _authenticationService.Logout(email);
                return Ok(ApiResponse<object>.SuccessResponse("Logout successful", result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Logout failed", new { ex.Message }));
            }
        }

    }
}
