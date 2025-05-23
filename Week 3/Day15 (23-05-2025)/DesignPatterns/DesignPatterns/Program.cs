using DesignPatterns.Singleton_DesignPattern;
using DesignPatterns.Factory_DesignPattern;
using DesignPatterns.AbstractFactory_DesignPattern;

namespace DesignPatterns
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Singleton singleton = new Singleton();
            //singleton.Run();

            //Factory factory = new Factory();
            //factory.Run();

            AbstractFactory abstractFactory = new AbstractFactory();
            abstractFactory.Run();
        }
    }
}
