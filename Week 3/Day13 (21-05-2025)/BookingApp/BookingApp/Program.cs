using BookingApp.Interface;
using BookingApp.UserDefinedException;
using BookingApp.Repository;
using BookingApp.Service;

namespace BookingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepository<int, Appointment> repo = new AppointmentRepository();
            IAppointmentService service = new AppointmentService(repo);
            AppointmentManger manage = new AppointmentManger(service);
            manage.Start();
        }
    }
}
