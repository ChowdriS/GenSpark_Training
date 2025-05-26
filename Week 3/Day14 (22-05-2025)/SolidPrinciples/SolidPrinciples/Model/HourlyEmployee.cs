using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;

namespace SolidPrinciples.Model
{
    public class HourlyEmployee : Employee
    {
        public decimal HourlyRate { get; set; }

        public override string GetEmployeeType() => $"Hourly, Rate: {HourlyRate}/hr";
    }
}
