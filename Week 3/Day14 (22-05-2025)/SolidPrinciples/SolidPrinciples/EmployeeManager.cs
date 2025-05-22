using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;
using SolidPrinciples.Model;

namespace SolidPrinciples
{
    public class ManageEmployee
    {
        private readonly IEmployeeService _service;

        public ManageEmployee(IEmployeeService service)
        {
            _service = service;
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("\n1. Add Full-Time Employee\n2. Add Hourly Employee\n3. View All Employees\n4. Exit");
                Console.Write("Choose: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddFullTimeEmployee();
                        break;
                    case "2":
                        AddHourlyEmployee();
                        break;
                    case "3":
                        ShowAllEmployees();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid Option.");
                        break;
                }
            }
        }

        private void AddFullTimeEmployee()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());

            var employee = new FullTimeEmployee { Name = name, Salary = salary };
            _service.AddEmployee(employee);
            Console.WriteLine("Full-Time Employee added.");
        }

        private void AddHourlyEmployee()
        {
            Console.Write("Name: ");
            string? name = Console.ReadLine();
            Console.Write("Hourly Rate: ");
            decimal rate = decimal.Parse(Console.ReadLine());

            var employee = new HourlyEmployee { Name = name, HourlyRate = rate };
            _service.AddEmployee(employee);
            Console.WriteLine("Hourly Employee added.");
        }

        private void ShowAllEmployees()
        {
            var employees = _service.GetAllEmployees();
            if (employees.Count == 0)
                Console.WriteLine("No employees found.");
            else
                employees.ForEach(emp => Console.WriteLine(emp));
        }
    }
}
