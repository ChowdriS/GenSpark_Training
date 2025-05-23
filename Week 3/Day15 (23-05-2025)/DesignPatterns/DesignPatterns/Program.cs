using DesignPatterns.Singleton_DesignPattern;
using DesignPatterns.Factory_DesignPattern;
using DesignPatterns.AbstractFactory_DesignPattern;
using DesignPatterns.Proxy_DesignPattern;
using DesignPatterns.Adapter_DesignPattern;
using DesignPatterns.FlyWeight_DesignPattern;

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

            //AbstractFactory abstractFactory = new AbstractFactory();
            //abstractFactory.Run();

            Proxy proxy = new Proxy();
            proxy.Run();

            //Adapter adapter = new Adapter();
            //adapter.Run();

            //FlyWeight flyWeight = new FlyWeight();
            //flyWeight.Run();
        }
    }
}
