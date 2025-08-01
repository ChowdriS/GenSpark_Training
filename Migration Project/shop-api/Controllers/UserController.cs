using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop_api.Interfaces;
using shop_api.Models.DTO;

namespace shop_api.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IOtherFunctionalities _otherFuntionailities;

        public UserController(IAuthenticationService authenticationService,
                                IUserService userService,
                                IOtherFunctionalities otherFuntionailities)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _otherFuntionailities = otherFuntionailities;
        }

        [HttpPost("admin")]
        

        public async Task<IActionResult> AddAdmin([FromBody] UserAddRequestDTO dto)
        {
            try
            {
                var user = await _userService.AddAdmin(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAddRequestDTO dto)
        {
            try
            {
                var user = await _userService.AddUser(dto);
                return Ok( user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message );
            }
        }

        [HttpGet("me")]
        
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var userId = _otherFuntionailities.GetLoggedInUserId(User);
                var user_me = await _userService.GetMe(userId);
                return Ok(user_me);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
