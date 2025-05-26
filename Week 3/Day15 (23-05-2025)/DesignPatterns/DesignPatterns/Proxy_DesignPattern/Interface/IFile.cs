using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Proxy_DesignPattern.Model;

namespace DesignPatterns.Proxy_DesignPattern.Interface
{
    public interface IFile
    {
        void Read(User user);
    }
}
