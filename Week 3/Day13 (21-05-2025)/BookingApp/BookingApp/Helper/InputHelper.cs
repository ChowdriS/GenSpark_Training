using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Helper
{
    public static class InputHelper
    {
        public static int ReadInt(string prompt)
        {
            int result;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine($"Invalid input. Please enter an integer -> ");
            }
            return result;
        }

        public static double ReadDouble(string prompt)
        {
            double result;
            Console.Write(prompt);
            while (!double.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine($"Invalid input. Please enter a number -> ");
            }
            return result;
        }

        public static DateTime ReadDate(string prompt, string format = "yyyy-MM-dd HH:mm")
        {
            DateTime result;
            Console.Write($"{prompt} (Format: {format})");
            while (!DateTime.TryParseExact(Console.ReadLine(), format, null, System.Globalization.DateTimeStyles.None, out result))
            {
                Console.WriteLine($"Invalid date. Please enter the date in the format {format}:");
            }
            return result;
        }

        public static string ReadString(string prompt, bool allowEmpty = false)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            while (!allowEmpty && string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Input cannot be empty. Please try again:");
                input = Console.ReadLine();
            }
            return input ?? string.Empty;
        }

        public static string? GetOptionalString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static DateTime? GetOptionalDate(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (DateTime.TryParse(input, out var date))
                return date;
            return null;
        }

        public static int? GetOptionalInt(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int value))
                return value;
            return null;
        }

        public static double? GetOptionalDouble(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double value))
                return value;
            return null;
        }
    }

}
