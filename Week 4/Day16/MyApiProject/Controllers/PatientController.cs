using Microsoft.AspNetCore.Mvc;
using MyApiProject.Models;
using MyApiProject.Dto;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private static List<Patient> patients = new List<Patient>();
        private static int nextId = 1;

        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetAll()
        {
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public ActionResult<Patient> Get(int id)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();
            return Ok(patient);
        }

        [HttpPost]
        public ActionResult<Patient> Create(PatientDto newpatient)
        {
            Patient patient = new Patient();
            patient.Id = nextId++;
            patient.Name = newpatient.Name;
            patient.Age = newpatient.Age;
            patient.Gender = newpatient.Gender;
            patient.Problem = newpatient.Problem;

            patients.Add(patient);
            return Created("", patient);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, PatientDto updatedPatient)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();

            patient.Name = updatedPatient.Name;
            patient.Age = updatedPatient.Age;
            patient.Gender = updatedPatient.Gender;
            patient.Problem = updatedPatient.Problem;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var patient = patients.FirstOrDefault(p => p.Id == id);
            if (patient == null)
                return NotFound();

            patients.Remove(patient);
            return NoContent();
        }
    }
}
