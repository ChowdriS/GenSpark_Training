using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;

namespace SolidPrinciples.Model
{
    public class FullTimeEmployee : Employee, ISalaryCalculator
    {
        public int MonthlySalary { get; set; }

        public FullTimeEmployee(int id, string name, int monthlySalary)
            : base(id, name)
        {
            MonthlySalary = monthlySalary;
        }

        public int CalculateSalary()
        {
            return MonthlySalary;
        }
    }
}
