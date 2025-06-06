using System.Threading.Tasks;
using EventBookingApi.Interface;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
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
                return Ok(ApiResponse<object>.SuccessResponse("Token succesfully generated",data));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Token generation is failed", new { ex.Message}));
            }
        }
    }
}
