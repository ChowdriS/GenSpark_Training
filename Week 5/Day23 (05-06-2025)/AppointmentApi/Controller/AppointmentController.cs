using AppointmentApi.Interface;
using AppointmentApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPut("cancel/{appointmentNumber}")]
        [Authorize(Policy = "DoctorWith3YearsExp")]
        public async Task<IActionResult> CancelAppointment(string appointmentNumber)
        {
            try
            {
                var result = await _appointmentService.CancelAppointment(appointmentNumber);
                return Ok("Appointment cancelled successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AppointmentRequestDTO dto)
        {
            try
            {
                var result = await _appointmentService.AddAppointment(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _appointmentService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
