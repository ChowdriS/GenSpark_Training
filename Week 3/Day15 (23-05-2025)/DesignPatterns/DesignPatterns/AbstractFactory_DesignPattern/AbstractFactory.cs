using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.AbstractFactory_DesignPattern.Interface;
using DesignPatterns.AbstractFactory_DesignPattern.Factory;

namespace DesignPatterns.AbstractFactory_DesignPattern
{
    public class AbstractFactory
    {
        public void Run()
        {
            //IShirtFactory factory = new NikeFactory();
            IShirtFactory factory = new PoloFactory();
            BrandFactory BrandProducts = new BrandFactory(factory);
            BrandProducts.OrderShirt();
        }

    }
}
