using BankingAppApi.Interface;
using BankingAppApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerAddRequestDTO customerDto)
        {
            try
            {
                if (customerDto == null || string.IsNullOrWhiteSpace(customerDto.Name))
                    return BadRequest("Customer name is required.");

                var result = await _customerService.AddCustomerAsync(customerDto);
                return Created("", result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the customer: {ex.Message}");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                    return NotFound($"Customer with ID {customerId} not found.");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the customer: {ex.Message}");
            }
        }

        [HttpGet("{customerId}/accounts")]
        public async Task<IActionResult> GetAccountsByCustomerId(int customerId)
        {
            try
            {
                var accounts = await _customerService.GetAccountsByCustomerIdAsync(customerId);
                if (accounts == null || accounts.Count == 0)
                    return NotFound($"No accounts found for customer ID {customerId}.");

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving accounts: {ex.Message}");
            }
        }
    }
}
