using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.AbstractFactory_DesignPattern.Interface;
using DesignPatterns.AbstractFactory_DesignPattern.Model;

namespace DesignPatterns.AbstractFactory_DesignPattern.Factory
{
    public class AddidasFactory : IShirtFactory
    {
        public IShirt CreateShirt()
        {
            return new AddidasShirt();
        }
    }
}
