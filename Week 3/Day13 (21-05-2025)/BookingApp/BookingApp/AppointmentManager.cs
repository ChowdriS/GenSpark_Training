using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Helper;
using BookingApp.Interface;
using BookingApp.UserDefinedException;

namespace BookingApp
{
    public class AppointmentManger
    {
        private IAppointmentService _service;

        public AppointmentManger(IAppointmentService service)
        {
            _service = service;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n1. Add Appointment\n2. Search Appointment\n3. Upcoming\n4. Past\n5. Exit");
                var input = InputHelper.ReadString("Enter your choice : ");
                Console.Clear();
                switch (input)
                {
                    case "1": AddAppointment(); break;
                    case "2": SearchAppointment(); break;
                    case "3": ShowUpcoming(); break;
                    case "4": ShowPast(); break;
                    case "5": return;
                    default: Console.WriteLine("Invalid input"); break;
                }
            }
        }

        private void AddAppointment()
        {
            Console.WriteLine("\n===Add Appointment===\n");
            string name = InputHelper.ReadString("Enter Patient Name : ");

            int age = InputHelper.ReadInt("Enter Age : ");
            DateTime date;
            while (true) { 
                date = InputHelper.ReadDate("Enter Appointment Date (yyyy-MM-dd HH:mm) : ");
                if (date <= DateTime.Now)
                {
                    Console.WriteLine("Invalid date. Appointment must be scheduled in the future.");
                }
                else
                {
                    break;
                }
            }

            string reason = InputHelper.ReadString("Enter Reason for Visit : ");

            var appointment = new Appointment
            {
                PatientName = name,
                PatientAge = age,
                AppointmentDate = date,
                Reason = reason
            };

            int id = _service.AddAppointment(appointment);
            Console.WriteLine(id != -1 ? $"Appointment added with ID: {id}" : "Failed to add appointment.");
        }


        private void SearchAppointment()
        {
            Console.WriteLine("\n===Search Appointment===\n");
            var model = new AppointmentSearchModel();

            model.PatientName = InputHelper.GetOptionalString("Search by Name (optional):");

            model.AppointmentDate = InputHelper.GetOptionalDate("Search by Date (optional, yyyy-MM-dd):");

            int? minAge = InputHelper.GetOptionalInt("Search by Min Age (optional):");
            int? maxAge = InputHelper.GetOptionalInt("Search by Max Age (optional):");

            if (minAge.HasValue && maxAge.HasValue)
            {
                model.AgeRange = new Range<int> { MinVal = minAge.Value, MaxVal = maxAge.Value };
            }
            else if (minAge < maxAge)
            {
                Console.WriteLine("It is Not a Valid Range. Skipping the age based search...");
            }
            var results = _service.SearchAppointments(model);

            if (results == null || results.Count == 0)
            {
                Console.WriteLine("No appointments found.");
            }
            else
            {
                foreach (var app in results)
                {
                    Console.WriteLine(app);
                }
            }
        }


        private void ShowUpcoming()
        {
            Console.WriteLine("\n===Upcoming Appointments===\n");
            var list = _service.GetNextAppointments();
            foreach (var a in list) Console.WriteLine(a);
        }

        private void ShowPast()
        {
            Console.WriteLine("\n===Past Appointments===\n");
            var list = _service.GetPastAppointments();
            foreach (var a in list) Console.WriteLine(a);
        }
    }

}
