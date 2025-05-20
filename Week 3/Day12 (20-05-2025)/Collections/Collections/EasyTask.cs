using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal class EasyTask
    {
        public List<string> AddEmployee()
        {
            List<string> list = new List<string>();
            while (true)
            {
                string? name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(name))
                    break;
                list.Add(name);
            }
            return list;
        }

        public void FindIndexOfEmployee(List<string> list, string searchName)
        {
            int position =  list.FindIndex(name => name == searchName);
            if (position != -1)
                Console.WriteLine($"\"{searchName}\" is at position {position + 1} for promotion.");
            else
                Console.WriteLine($"\"{searchName}\" is not found in the promotion list.");
        }

        public void DisplayList(List<string> list) 
        {
            foreach (string emp in list)
            {
                Console.WriteLine(emp);
            }
        }

        public void SortAndPrint(List<string> list)
        {
            Console.WriteLine("\nPromoted employee list (in ascending order):");
            list.Sort();
            DisplayList(list);
        }
        public void CollectionWithEmployeeName()
        {
            //Task 1: Create a C# console application that will 
            //take employee names in the order in which they are eligible for promotion
            List<string> promotionList = new List<string>();
            Console.WriteLine("Please enter the employee names in the order of their eligibility for promotion (Enter blank to stop):");
            promotionList = AddEmployee();

            // Task 2: Find position of an employee
            Console.WriteLine("\nPlease enter the name of the employee to check promotion position:");
            string? searchName = Console.ReadLine();
            FindIndexOfEmployee(promotionList,searchName);

            // Task 3: Memory optimization
            Console.WriteLine($"\nThe current size (capacity) of the collection is {promotionList.Capacity}");
            Console.WriteLine($"The number of elements (count) is {promotionList.Count}");
            promotionList.TrimExcess();
            Console.WriteLine($"The size after removing the extra space is {promotionList.Capacity}");

            // Task 4: Sort and print all names
            SortAndPrint(promotionList);
        }
    }
}
