using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns.Proxy_DesignPattern.Model
{
    public class User
    {
        public string Username { get; }
        public string Role { get; }

        public User(string username, string role)
        {
            Username = username;
            Role = role;
        }

        public override string ToString() =>
            $"\nUser:{Username}-Role:{Role}";
    }
}
