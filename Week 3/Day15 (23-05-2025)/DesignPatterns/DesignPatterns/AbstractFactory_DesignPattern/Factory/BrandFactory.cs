using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.AbstractFactory_DesignPattern.Interface;

namespace DesignPatterns.AbstractFactory_DesignPattern.Factory
{
    public class BrandFactory
    {
        private readonly IShirtFactory _shirt;
        public BrandFactory(IShirtFactory brand)
        {
            _shirt = brand;
        }

        public void OrderShirt()
        {
            IShirt brandshirt = _shirt.CreateShirt();
            brandshirt.DeliveryShirt();
        }
    }
}
