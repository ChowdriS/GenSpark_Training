using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Singleton_DesignPattern
{
    public class Logger
    {
        private static Logger? _instance;
        private Logger()
        {
            Console.WriteLine("Logger Is Initiated!!");
        }

        public static Logger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }
            return _instance;
        }

        public void Log(string message)
        {
            Console.WriteLine($"Message : {message}");
        }
    }
    public class Singleton
    {
        public void Run() { 
            Logger logger1 = Logger.GetInstance();
            Logger logger2 = Logger.GetInstance();

            logger1.Log("Logger 1 here!");
            logger2.Log("Logger 2 here!");
        }
    }
}
