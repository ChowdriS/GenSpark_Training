using AppointmentApi.Interface;
using AppointmentApi.Models.DTO;
using AppointmentApi.Pipes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }
        [HttpPost]
        [CustomExceptionFilter]
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            // try
            // {
            //     var result = await _authenticationService.Login(loginRequest);
            //     return Ok(result);
            // }
            // catch (Exception e)
            // {
            //     // _logger.LogError(e.Message);
            //     return Unauthorized(e.Message);
            // }
            var result = await _authenticationService.Login(loginRequest);
            return Ok(result);
        }
    }
}
