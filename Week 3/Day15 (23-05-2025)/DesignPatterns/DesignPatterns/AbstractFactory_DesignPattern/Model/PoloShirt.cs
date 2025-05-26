using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.AbstractFactory_DesignPattern.Interface;

namespace DesignPatterns.AbstractFactory_DesignPattern.Model
{
    public class PoloShirt : IShirt
    {
        public void DeliveryShirt()
        {
            Console.WriteLine("Polo Shirt is ready for delivery!!");
        }
    }
}
