using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class Employee
    {
        int id, age;
        string? name;
        double salary;

        public Employee() {}

        public Employee(int id, int age, string? name, double salary)
        {
            this.id = id;
            this.age = age;
            this.name = name;
            this.salary = salary;
        }

        public void TakeEmployeeDetailsFromUser(UserInput userinput)
        {
            Console.Write("Please enter the employee ID : ");
            id = userinput.getUserInt();
            Console.Write("Please enter the employee name : ");
            name = Console.ReadLine();
            Console.Write("Please enter the employee age : ");
            age = userinput.getUserInt();
            Console.Write("Please enter the employee salary : ");
            salary = userinput.getUserDouble();
        }

        public override string ToString()
        {
            return "Employee ID : " + id + " - Name : " + name + " - Age : " + age + " - Salary : " + salary;
        }

        public int Id { get => id; set => id = value; }
        public int Age { get => age; set => age = value; }
        public string Name { get => name; set => name = value; }
        public double Salary { get => salary; set => salary = value; }
    }
}
