using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class MediumTask
    {
        private Dictionary<int, Employee> employeeDict = new Dictionary<int, Employee>();
        private UserInput userInput = new UserInput();

        public void AddEmployee()
        {
            Console.Write("Enter number of Employees to add: ");
            int n = userInput.getUserInt();

            for (int i = 0; i < n; i++)
            {
                Employee emp = new Employee();
                emp.TakeEmployeeDetailsFromUser(userInput);

                if (!employeeDict.ContainsKey(emp.Id))
                    employeeDict.Add(emp.Id, emp);
                else
                    Console.WriteLine("Duplicate ID. Skipping this employee.");
                Console.WriteLine("Employee is Added!!");
            }
        }

        public void PrintAllEmployee()
        {
            List<Employee> List = employeeDict.Values.ToList();
            foreach (var emp in List)
            {
                Console.WriteLine(emp.ToString());
            }
        }

        public void ModifyEmployeeByID()
        {
            PrintAllEmployee();
            Console.Write("\nEnter employee ID to modify: ");
            int id = userInput.getUserInt();

            Employee OldEmployee = employeeDict[id];
            if (OldEmployee == null)
                Console.WriteLine("No Employee is associated with this given id!!\n");
            else
            {
                bool changed = false;
                Console.WriteLine("Enter new details of the Employee - (Leave empty to unchange!!)");
                Console.Write($"OldName - {OldEmployee.Name} => Enter new name: ");
                string? name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name)) { 
                    OldEmployee.Name = name;
                    changed = true;
                }
                
                Console.Write($"OldAge - {OldEmployee.Age} => Enter new age: ");
                if (int.TryParse(Console.ReadLine(), out int newAge)) {
                    OldEmployee.Age = newAge;
                    changed = true;
                }

                Console.Write($"OldSalary - {OldEmployee.Salary} => Enter new salary: ");
                if (double.TryParse(Console.ReadLine(), out double newSalary)) {
                    OldEmployee.Salary = newSalary;
                    changed = true;
                }
                if(!changed)
                    Console.WriteLine("Employee details updated successfully!!");
                else
                    Console.WriteLine("Nothing is Updated!!");
            }
        }
        public void SortAndDisplayBySalary()
        {
            List<Employee> empList = employeeDict.Values.ToList();
            empList.Sort((e1, e2) => e1.Salary.CompareTo(e2.Salary));

            Console.WriteLine("\n--- Sorted employeeDict by Salary ---");
            PrintAllEmployee();
        }
        public void DeleteEmployeeByID()
        {
            Console.Write("\nEnter employee ID to delete: ");
            int id = userInput.getUserInt();

            Employee OldEmployee = employeeDict[id];
            if (OldEmployee == null)
                Console.WriteLine("No Employee is associated with this given id!!\n");
            else
            {
                employeeDict.Remove(id);
                Console.WriteLine($"EmployeeID - {id} is Deleted!\n");
            }
        }

        public void FindEmployeeById()
        {
            Console.Write("\nEnter employee ID to search: ");
            int id = userInput.getUserInt();

            var result = employeeDict.Values.Where(e => e.Id == id).FirstOrDefault();
            if (result != null)
                Console.WriteLine("Found: " + result);
            else
                Console.WriteLine("Employee not found.");
        }

        public void FindEmployeeByName()
        {
            Console.Write("\nEnter employee name to search: ");
            string? name = Console.ReadLine();

            var results = employeeDict.Values
                            .Where(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();

            Console.WriteLine($"\nEmployees with name \"{name}\":");
            if (results.Count == 0)
                Console.WriteLine("No Employee found.");
            else
                results.ForEach(e => Console.WriteLine(e.ToString()));
        }

        public void FindOlderThanGivenEmployee()
        {
            Console.Write("\nEnter employee ID to compare age: ");
            int id = userInput.getUserInt();

            if (!employeeDict.ContainsKey(id))
            {
                Console.WriteLine("Employee ID not found.");
                return;
            }

            var refEmp = employeeDict[id];
            var elders = employeeDict.Values
                            .Where(e => e.Age > refEmp.Age)
                            .ToList();

            Console.WriteLine($"\nemployeeDict older than {refEmp.Name} (Age {refEmp.Age}):");
            if (elders.Count == 0)
                Console.WriteLine("No elder employeeDict found.");
            else
                elders.ForEach(e => Console.WriteLine(e.ToString()));
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== Employee Management Menu ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Display All Employees");
                Console.WriteLine("3. Modify an Employee by ID");
                Console.WriteLine("4. Sort Employees by Salary");
                Console.WriteLine("5. Find Employee by ID");
                Console.WriteLine("6. Find Employees by Name");
                Console.WriteLine("7. Find Employees Older Than a Given Employee");
                Console.WriteLine("8. Delete the Employee by ID");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                int choice = userInput.getUserInt();
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("\n--- Add Employee ---");
                        AddEmployee();
                        break; 
                    case 2:
                        Console.WriteLine("\n--- Display All Employees ---");
                        PrintAllEmployee();
                        break; 
                    case 3:
                        Console.WriteLine("\n--- Modify an Employee by ID ---");
                        ModifyEmployeeByID();
                        break;
                    case 4:
                        Console.WriteLine("\n--- Sorted Employees by Salary ---");
                        SortAndDisplayBySalary();
                        break;
                    case 5:
                        Console.WriteLine("\n--- Find Employees by ID ---");
                        FindEmployeeById();
                        break;
                    case 6:
                        Console.WriteLine("\n--- Find Employees by Name ---");
                        FindEmployeeByName();
                        break;

                    case 7:
                        Console.WriteLine("\n--- Find Employees Older Than a Given Employee ---");
                        FindOlderThanGivenEmployee();
                        break;
                    case 8:
                        Console.WriteLine("\n--- Delete an Employee by ID ---");
                        DeleteEmployeeByID();
                        break;
                    case 9:
                        Console.WriteLine("Exiting application. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 6.");
                        break;
                }
            }
        }

    }
}
