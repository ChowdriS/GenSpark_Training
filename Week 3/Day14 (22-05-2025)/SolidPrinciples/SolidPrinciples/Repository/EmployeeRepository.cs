using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;
using SolidPrinciples.Model;

namespace SolidPrinciples.Repository
{
    public class EmployeeRepository : Repository<int, Employee>, IEmployeeRepository
    {
        private static int _currentId = 1001;

        public override void Add(Employee employee)
        {
            employee.Id = _currentId++;
            _items[employee.Id] = employee;
        }
    }
}
