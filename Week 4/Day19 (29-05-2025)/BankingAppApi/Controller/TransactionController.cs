using BankingAppApi.Interface;
using BankingAppApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransactionDepositRequestDTO dto)
        {
            try
            {
                if (dto == null || dto.ToAccountId <= 0 || dto.Amount <= 0)
                    return BadRequest("Invalid deposit request data.");

                var transaction = await _transactionService.DepositAsync(dto);
                return Created("", transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during deposit: {ex.Message}");
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] TransactionWithDrawRequestDTO dto)
        {
            try
            {
                if (dto == null || dto.FromAccountId <= 0 || dto.Amount <= 0)
                    return BadRequest("Invalid withdrawal request data.");

                var transaction = await _transactionService.WithdrawAsync(dto);
                return Created("", transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during withdrawal: {ex.Message}");
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransactionAddRequestDTO dto)
        {
            try
            {
                if (dto == null || dto.FromAccountId <= 0 || dto.ToAccountId <= 0 || dto.Amount <= 0)
                    return BadRequest("Invalid transfer request data.");

                var transaction = await _transactionService.TransferAsync(dto);
                return Created("", transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error during transfer: {ex.Message}");
            }
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccountId(long accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByAccountIdAsync(accountId);
                if (transactions == null || transactions.Count == 0)
                    return NotFound($"No transactions found for account ID {accountId}.");

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while retrieving transactions: {ex.Message}");
            }
        }
    }
}
