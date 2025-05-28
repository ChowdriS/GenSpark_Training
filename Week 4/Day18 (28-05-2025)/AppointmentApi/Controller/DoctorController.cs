using System.Threading.Tasks;
using AppointmentApi.Interface;
using AppointmentApi.Models;
using AppointmentApi.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetDoctorByName(string name)
        {
            try
            {
                var doctor = await _doctorService.GetDoctorByName(name);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorAddRequestDto dto)
        {
            try
            {
                var doctor = await _doctorService.AddDoctor(dto);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("speciality/{speciality}")]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctorsBySpeciality(string speciality)
        {
            try
            {
                var doctors = await _doctorService.GetDoctorsBySpeciality(speciality);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
