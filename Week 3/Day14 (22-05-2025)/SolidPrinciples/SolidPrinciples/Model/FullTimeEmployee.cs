using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;

namespace SolidPrinciples.Model
{
    public class FullTimeEmployee : Employee
    {
        public decimal Salary { get; set; }

        public override string GetEmployeeType() => $"Full-Time, Salary: {Salary}";
    }
}
