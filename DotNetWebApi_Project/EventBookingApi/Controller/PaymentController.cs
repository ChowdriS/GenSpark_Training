using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBookingApi.Interface;
using EventBookingApi.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controllers
{
    [ApiController]
    [Route("api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOtherFunctionalities _otherFunctionalities;

        public PaymentController(
            IPaymentService paymentService,
            IOtherFunctionalities otherFunctionalities)
        {
            _paymentService = paymentService;
            _otherFunctionalities = otherFunctionalities;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<PaymentDetailDTO>> GetPaymentById(Guid id)
        {
            try
            {
                var currentUserId = _otherFunctionalities.GetLoggedInUserId(User);
                var payment = await _paymentService.GetPaymentById(id, currentUserId);
                return Ok(payment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PaymentDetailDTO>>> GetPaymentsByUser(Guid userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsByUserId(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("event/{eventId}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<ActionResult<IEnumerable<PaymentDetailDTO>>> GetPaymentsByEvent(Guid eventId)
        {
            try
            {
                Guid? managerId = null;
                if (User.IsInRole("Manager"))
                {
                    managerId = _otherFunctionalities.GetLoggedInUserId(User);
                }
                
                var payments = await _paymentService.GetPaymentsByEventId(eventId, managerId);
                return Ok(payments);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
