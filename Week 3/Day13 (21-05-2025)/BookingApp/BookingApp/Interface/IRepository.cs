using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.UserDefinedException;

namespace BookingApp.Interface
{
    public interface IRepository<K, T>
    {
        ICollection<T> GetAll();
        T GetById(K id);
        T Add(T item);
        T Update(T item);
        T Delete(K id);
        ICollection<Appointment> GetUpcomingAppointments();
        ICollection<Appointment> GetPastAppointments();
    }

}
