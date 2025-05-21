using WholeApplication.Interface;
using WholeApplication.Models;
using WholeApplication.Models.WholeApplication.Models;
using WholeApplication.Repository;
using WholeApplication.Service;

namespace WholeApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            static void Main(string[] args)
            {
                IRepository<int, Employee> employeeRepository = new EmployeeRepository();
                IEmployeeService employeeService = new EmployeeService(employeeRepository);
                ManageEmployee manageEmployee = new ManageEmployee(employeeService);
                manageEmployee.Start();
            }
        }
    }
}