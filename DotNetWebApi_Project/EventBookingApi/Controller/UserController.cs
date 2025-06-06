using System.Threading.Tasks;
using EventBookingApi.Context;
using EventBookingApi.Interface;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controller
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UserController(IAuthenticationService authenticationService,
                                IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAdmin([FromBody] UserAddRequestDTO dto)
        {
            try
            {
                var user = await _userService.addAdmin(dto);
                return Ok(ApiResponse<object>.SuccessResponse("Admin succesfully added ",user));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("User creation is failed", new {ex.Message }));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAddRequestDTO dto)
        {
            try
            {
                var user = await _userService.addUser(dto);
                return Ok(ApiResponse<object>.SuccessResponse($"{user.Role} succesfully added ", user));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("User creation is failed", new { ex.Message }));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(string email, UserUpdateRequestDTO dto)
        {
            try
            {
                var user = await _userService.updateUser(email, dto);
                return Ok(ApiResponse<object>.SuccessResponse("User succesfully updated", user));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("User updation is failed", new { ex.Message }));
            }
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword(string email, ChangePasswordDTO dto)
        {
            try
            {
                var user = await _userService.changePasssword(email, dto);
                return Ok(ApiResponse<object>.SuccessResponse("Password succesfully Changed", user));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Password updation is failed", new { ex.Message }));
            }
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                var user = await _userService.deleteUser(email);
                return Ok(ApiResponse<object>.SuccessResponse("User is successfully deleted!",user));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("User deletion is failed", new {ex.Message }));
            }
        }
    }
}
