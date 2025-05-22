using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;
using SolidPrinciples.Model;

namespace SolidPrinciples.Service
{
    public class ConsoleEmployeePrinter : IEmployeePrinter
    {
        public void PrintSalary(Employee employee, ISalaryCalculator salaryCalculator)
        {
            Console.WriteLine($"Employee: {employee.Name} | Salary: {salaryCalculator.CalculateSalary()}");
        }
    }
}
