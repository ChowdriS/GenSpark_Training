using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Interface;
using BookingApp.UserDefinedException;
using BookingApp.Repository;

namespace BookingApp.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<int, Appointment> _appointmentRepo;

        public AppointmentService(IRepository<int, Appointment> repo)
        {
            _appointmentRepo = repo;
        }

        public int AddAppointment(Appointment appointment)
        {
            try
            {
                var result = _appointmentRepo.Add(appointment);
                return result.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        public List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel)
        {
            try
            {
                var results = _appointmentRepo.GetAll();

                results = FilterByName(results, searchModel.PatientName);
                results = FilterByDate(results, searchModel.AppointmentDate);
                results = FilterByAge(results, searchModel.AgeRange);

                return results.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        private ICollection<Appointment> FilterByName(ICollection<Appointment> appointments, string? name)
        {
            if (string.IsNullOrWhiteSpace(name) || name == null) return appointments;
            return appointments.Where(app => app.PatientName.ToLower().Contains(name.ToLower())).ToList();
        }

        private ICollection<Appointment> FilterByDate(ICollection<Appointment> appointments, DateTime? date)
        {
            if (date == null) return appointments;
            return appointments.Where(app => app.AppointmentDate.Date == date.Value.Date).ToList();
        }

        private ICollection<Appointment> FilterByAge(ICollection<Appointment> appointments, Range<int>? ageRange)
        {
            if (ageRange == null) return appointments;
            return appointments.Where(a =>
                a.PatientAge >= ageRange.MinVal && a.PatientAge <= ageRange.MaxVal).ToList();
        }

        public List<Appointment> GetNextAppointments() => _appointmentRepo.GetUpcomingAppointments().ToList();
        public List<Appointment> GetPastAppointments() => _appointmentRepo.GetPastAppointments().ToList();
    }

}
