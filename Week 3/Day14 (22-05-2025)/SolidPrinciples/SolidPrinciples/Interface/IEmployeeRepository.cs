using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Model;

namespace SolidPrinciples.Interface
{
    public interface IEmployeeRepository : IRepository<int, Employee> { }
}
