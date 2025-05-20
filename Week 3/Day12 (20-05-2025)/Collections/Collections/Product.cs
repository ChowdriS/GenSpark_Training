using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    internal class Product
    {
        private UserInput userinput = new UserInput();
        private Dictionary<string, double> ProductMap = new Dictionary<string, double>();

        void AddProduct()
        {
            string? Name = Console.ReadLine();
            Double Price = userinput.getUserDouble();

            if (ProductMap.ContainsKey(Name))
            {
                Console.WriteLine("Product is Alreadly Available!!");
            }
            else
            {
                ProductMap[Name] = Price;
            }
        }

        void GetAllProducts()
        {
            foreach (var product in ProductMap)
            {
                Console.WriteLine($"{product.Key} - {product.Value}");
            }
        }

        void DisplayProductByName(string name)
        {
            Console.WriteLine($"{name} - {ProductMap[name]}");
        }

        void GetProductByName()
        {
            string? name = Console.ReadLine();
            if (ProductMap.ContainsKey(name)){
                DisplayProductByName(name);
            }
            else
            {
                Console.WriteLine("The given name is not Available!!");
            }
        }



    }
}
