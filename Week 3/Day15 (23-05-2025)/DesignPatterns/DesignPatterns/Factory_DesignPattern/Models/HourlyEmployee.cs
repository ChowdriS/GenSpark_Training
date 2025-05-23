using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Factory_DesignPattern.Interface;

namespace DesignPatterns.Factory_DesignPattern.Models
{
    public class HourlyEmployee : IEmployee
    {

        public void GetDetails()
        {
            Console.WriteLine($"Hourly Employee!!");
        }
    }
}
