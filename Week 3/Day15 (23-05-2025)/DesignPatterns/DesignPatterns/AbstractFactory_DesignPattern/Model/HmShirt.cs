using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.AbstractFactory_DesignPattern.Interface;

namespace DesignPatterns.AbstractFactory_DesignPattern.Model
{
    public class HmShirt : IShirt
    {
        public void DeliveryShirt()
        {
            Console.WriteLine("H&M Shirt is ready for delivery!!");
        }
    }
}
