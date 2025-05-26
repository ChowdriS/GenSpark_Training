using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Factory_DesignPattern.Interface;

namespace DesignPatterns.Factory_DesignPattern
{
    public class Factory
    {
        public void Run()
        {
            IEmployee employee1 = EmployeeFactory.CreateEmployee("Fulltime");
            employee1.GetDetails();
            IEmployee employee2 = EmployeeFactory.CreateEmployee("Hourly");
            employee2.GetDetails();
        }
    }
}
