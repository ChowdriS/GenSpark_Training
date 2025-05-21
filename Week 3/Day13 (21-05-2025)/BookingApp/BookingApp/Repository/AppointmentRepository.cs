using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Interface;
using BookingApp.UserDefinedException;

namespace BookingApp.Repository
{
    public class AppointmentRepository : Repository<int, Appointment>
    {
        private static int _currentId = 1000;

        protected override int GenerateID()
        {
            return ++_currentId;
        }

        public override ICollection<Appointment> GetAll()
        {
            if (_items.Count == 0)
            {
                throw new CollectionEmptyException("Appointments are Empty!");
            }
            return _items;
        }

        public override Appointment GetById(int id)
        {
            var appointment = _items.FirstOrDefault(e => e.Id == id);
            if (appointment == null)
            {
                throw new KeyNotFoundException("appointment not found");
            }
            return appointment;
        }

        public override ICollection<Appointment> GetUpcomingAppointments()
        {
            return _items
                .Where(a => a.AppointmentDate >= DateTime.Now)
                .OrderBy(a => a.AppointmentDate)
                .Take(5)
                .ToList();
        }

        public override ICollection<Appointment> GetPastAppointments()
        {
            return _items
                .Where(a => a.AppointmentDate < DateTime.Now)
                .OrderByDescending(a => a.AppointmentDate)
                .Take(5)
                .ToList();
        }
    }

}
