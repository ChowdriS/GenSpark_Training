using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.UserDefinedException;

namespace BookingApp.Interface
{
    public interface IAppointmentService
    {
        int AddAppointment(Appointment appointment);
        List<Appointment>? SearchAppointments(AppointmentSearchModel searchModel);
        List<Appointment> GetNextAppointments();
        List<Appointment> GetPastAppointments();
    }

}
