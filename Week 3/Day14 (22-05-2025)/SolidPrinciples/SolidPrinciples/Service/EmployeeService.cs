using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidPrinciples.Interface;
using SolidPrinciples.Model;
using SolidPrinciples.Repository;

namespace SolidPrinciples.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public void AddEmployee(Employee employee) => _repository.Add(employee);

        public List<Employee> GetAllEmployees() => _repository.GetAll();
    }
}
