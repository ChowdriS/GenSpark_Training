using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Factory_DesignPattern.Interface;
using DesignPatterns.Factory_DesignPattern.Models;

namespace DesignPatterns.Factory_DesignPattern
{
    public static class EmployeeFactory
    {
        public static IEmployee CreateEmployee(string type)
        {
            return type.ToLower() switch
            {
                "fulltime" => new FulltimeEmployee(),
                "hourly" => new HourlyEmployee(),
                _ => throw new Exception("Invalid Employee Type")
            }; 
        }
    }
}
