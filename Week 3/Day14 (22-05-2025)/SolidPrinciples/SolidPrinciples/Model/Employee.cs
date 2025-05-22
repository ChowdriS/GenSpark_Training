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
        public string Name { get; set; }

        protected Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
