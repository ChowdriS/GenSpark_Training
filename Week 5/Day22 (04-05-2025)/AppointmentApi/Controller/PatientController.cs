using System.Threading.Tasks;
using AppointmentApi.Interface;
using AppointmentApi.Models.DTO;
using AppointmentApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient([FromBody]PatientAddRequestDTO dto)
        {
            try
            {
                var patient = await _patientService.CreatePatient(dto);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}