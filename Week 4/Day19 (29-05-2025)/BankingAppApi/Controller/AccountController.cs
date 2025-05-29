using BankingAppApi.Interface;
using BankingAppApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] AccountAddRequestDTO dto)
        {
            try
            {
                var account = await _accountService.AddAccountAsync(dto);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{AccountNumber}")]
        public async Task<IActionResult> GetAccoutById(long AccountNumber)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(AccountNumber);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{AccountNumber}/balance")]
        public async Task<IActionResult> GetAccountBalance(long AccountNumber)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(AccountNumber);
                return Ok(account?.Balance);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        
    }
}
