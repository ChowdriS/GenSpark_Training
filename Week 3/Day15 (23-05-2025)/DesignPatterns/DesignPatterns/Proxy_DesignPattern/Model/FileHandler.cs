﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns.Proxy_DesignPattern.Interface;

namespace DesignPatterns.Proxy_DesignPattern.Model
{
    public class FileHandler : IFile
    {
        private string _fileName;

        public FileHandler(string fileName)
        {
            _fileName = fileName;
        }

        public void Read(User user)
        {
            Console.WriteLine($"Reading sensitive file content of {_fileName}...");
        }
    }
}
