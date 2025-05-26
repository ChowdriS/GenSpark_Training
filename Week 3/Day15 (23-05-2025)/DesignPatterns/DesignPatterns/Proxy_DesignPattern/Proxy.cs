using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Proxy_DesignPattern.Interface;
using DesignPatterns.Proxy_DesignPattern.Model;

namespace DesignPatterns.Proxy_DesignPattern
{
    public class ProxyFile : IFile
    {
        private FileHandler _realFile;
        private string _fileName;

        public ProxyFile(string fileName)
        {
            _fileName = fileName;
        }

        public void Read(User user)
        {
            if (_realFile == null)
            {
                _realFile = new FileHandler(_fileName);
            }

            switch (user.Role)
            {
                case "Admin":
                    _realFile.Read(user);
                    break;
                case "User":
                    Console.WriteLine($"User '{user.Username}' can only see metadata of '{_fileName}'");
                    break;
                case "Guest":
                    Console.WriteLine($"User '{user.Username}' does not have permission to read '{_fileName}'");
                    break;
                default:
                    Console.WriteLine($"Unknown role for user '{user.Username}'");
                    break;
            }
        }
    }

    public class Proxy
    {
        public void Run()
        {
            Console.WriteLine("===Secure File Access System===");

            IFile secureFile = new ProxyFile("data.txt");

            User admin = new User("PersonA", "Admin");
            Console.WriteLine(admin);
            secureFile.Read(admin);

            User regularUser = new User("PersonB", "User");
            Console.WriteLine(regularUser);
            secureFile.Read(regularUser);

            User guest = new User("PersonC", "Guest");
            Console.WriteLine(guest);
            secureFile.Read(guest);
        }
    }
}
