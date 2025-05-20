using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    public class UserInput
    {
        public int getUserInt()
        {
            int num;
            while (!int.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid Input! Please Enter Int value...");
            }
            return num;
        }
        public double getUserDouble()
        {
            double num;
            while (!double.TryParse(Console.ReadLine(), out num))
            {
                Console.WriteLine("Invalid Input! Please Enter Int value...");
            }
            return num;
        }
    }
}
