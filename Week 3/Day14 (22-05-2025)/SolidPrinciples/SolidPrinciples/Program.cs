using SolidPrinciples.Interface;
using SolidPrinciples.Model;
using SolidPrinciples.Service;

namespace SolidPrinciples
{

    public class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>
            {
                new FullTimeEmployee(1, "raj", 60000),
                new HourlyEmployee(2, "kamal", 160, 500)
            };

            IEmployeePrinter printer = new ConsoleEmployeePrinter();

            foreach (var emp in employees)
            {
                ISalaryCalculator? calculator = emp as ISalaryCalculator;
                if (calculator != null)
                {
                    printer.PrintSalary(emp, calculator);
                }
            }
        }
    }

}
