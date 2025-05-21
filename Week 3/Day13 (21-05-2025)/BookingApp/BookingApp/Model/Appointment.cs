using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Helper;

namespace BookingApp.UserDefinedException
{
    public class Appointment : IComparable<Appointment>, IEquatable<Appointment>
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int PatientAge { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Reason { get; set; } = string.Empty;

        public int CompareTo(Appointment? other)
        {
            return this.Id.CompareTo(other?.Id);
        }

        public bool Equals(Appointment? other)
        {
            return this.Id == other?.Id;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {PatientName}, Age: {PatientAge}, Date: {AppointmentDate}, Reason: {Reason}";
        }
    }

}
