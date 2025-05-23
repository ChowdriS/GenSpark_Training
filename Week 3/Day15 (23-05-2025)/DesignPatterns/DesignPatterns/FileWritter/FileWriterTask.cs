using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatterns.FileWritter
{
    public class FileManager
    {
        private static FileManager? _instance = null;
        private StreamWriter _writer;
        private StreamReader _reader;
        private FileStream _fileStream;

        private FileManager(string filePath)
        {
            _fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _writer = new StreamWriter(_fileStream);
            _reader = new StreamReader(_fileStream);
        }

        public static FileManager GetInstance(string filePath)
        {
            if (_instance == null)
            {
                _instance = new FileManager(filePath);
            }
            return _instance;
        }

        public void WriteLine(string line)
        {
            _writer.WriteLine(line);
            _writer.Flush();
        }

        public string ReadToEnd()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            return _reader.ReadToEnd();
        }

        public void Close()
        {
            _writer?.Close();
            _reader?.Close();
            _fileStream?.Close();
        }
    }
    public class FileWritterTask
    {
        public void Run()
        {
            string filePath = "log.txt";

            FileManager fileManager = FileManager.GetInstance(filePath);
            fileManager.WriteLine("Line1");
            fileManager.WriteLine("Line2");

            string content = fileManager.ReadToEnd();
            Console.WriteLine("File Contents:\n" + content);

            fileManager.Close();
        }
    }
}
