using SolidPrinciples.Interface;
using SolidPrinciples.Model;
using SolidPrinciples.Repository;
using SolidPrinciples.Service;

namespace SolidPrinciples
{

    public class Program
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
