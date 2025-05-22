using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;

namespace SolidPrinciples.Model
{
    public class HourlyEmployee : Employee, ISalaryCalculator
    {
        public int HoursWorked { get; set; }
        public int HourlyRate { get; set; }

        public HourlyEmployee(int id, string name, int hoursWorked, int hourlyRate)
            : base(id, name)
        {
            HoursWorked = hoursWorked;
            HourlyRate = hourlyRate;
        }

        public int CalculateSalary()
        {
            return HoursWorked * HourlyRate;
        }
    }
}
