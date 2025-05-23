using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidPrinciples.Model
{
    public abstract class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public abstract string GetEmployeeType();
        public override string ToString() => $"ID: {Id}, Name: {Name}, Type: {GetEmployeeType()}";
    }
}
