using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IOtherFunctionalities _otherFunctionalities;

        public AuthController(IAuthenticationService authenticationService,
                             IOtherFunctionalities otherFunctionalities)
        {
            _authenticationService = authenticationService;
            _otherFunctionalities = otherFunctionalities;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserLoginRequestDTO dto)
        {
            try
            {
                var data = await _authenticationService.Login(dto);
                return Ok( data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message );
            }
        }

    }
}
