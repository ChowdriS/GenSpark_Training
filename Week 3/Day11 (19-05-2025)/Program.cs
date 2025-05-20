using System.Numerics;
using System.Xml.Linq;
using May19Morning;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    //1) create a program that will take name from user and greet the user
    public static void WelcomeUser()
    {
        string? name = Console.ReadLine();
        Console.WriteLine($"Welcome {name}!"); 
    }

    //2) Take 2 numbers from user and print the largest
    public static int GetIntValue()
    {
        string? input = Console.ReadLine();
        return Convert.ToInt32( input );
    }
    public static int GetLarger(int x, int y) => x > y ? x : y;
    public static void PrintLargest()
    {
        int x, y;
        x = GetIntValue();
        y = GetIntValue();
        int largest = GetLarger(x, y);
        Console.WriteLine("The Larger Number is " + largest);
    }
    //3) Take 2 numbers from user, check the operation user wants to perform(+,-,*,/). Do the operation and print the result
    public static void Calculator()
    {
        Console.Write("Enter first number: ");
        double num1 = GetIntValue();

        Console.Write("Enter second number: ");
        double num2 = GetIntValue();

        Console.Write("Enter operation (+, -, *, /): ");
        char operation = Console.ReadKey().KeyChar;
        Console.WriteLine();

        double result;

        switch (operation)
        {
            case '+':
                result = num1 + num2;
                Console.WriteLine("Result: " + result);
                break;

            case '-':
                result = num1 - num2;
                Console.WriteLine("Result: " + result);
                break;

            case '*':
                result = num1 * num2;
                Console.WriteLine("Result: " + result);
                break;

            case '/':
                if (num2 != 0)
                {
                    result = num1 / num2;
                    Console.WriteLine("Result: " + result);
                }
                else
                {
                    Console.WriteLine("Error: Division by zero is not allowed.");
                }
                break;

            default:
                Console.WriteLine("Invalid operation.");
                break;
        }
    }

    //4) Take username and password from user.Check if user name is "Admin" and password is "pass" if yes then print success message.
    //Give 3 attempts to user.In the end of eh 3rd attempt if user still is unable to provide valid creds then exit the application after print the message
    //"Invalid attempts for 3 times. Exiting...."
    public static void ValidCredential()
    {
        int attempts = 0;
        bool success = false;

        while (attempts < 3)
        {
            Console.Write("Enter username: ");
            string? username = Console.ReadLine();

            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if (username == "Admin" && password == "Pass")
            {
                Console.WriteLine("Login successful! Welcome, Admin.");
                success = true;
                break;
            }
            else
            {
                attempts++;
                Console.WriteLine("Invalid credentials. Attempts left: " + (3 - attempts));
            }
        }

        if (!success)
        {
            Console.WriteLine("Invalid attempts for 3 times. Exiting....");
        }
    }
    
    //5) Take 10 numbers from user and print the number of numbers that are divisible by 7
    public static void CountDivisibleBy7()
    {
        int[] numbers = new int[10];
        int count = 0;

        Console.WriteLine("Enter 10 numbers:");

        for (int i = 0; i < 10; i++)
        {
            Console.Write("Number " + (i + 1) + ": ");
            numbers[i] = GetIntValue();
        }

        foreach (int num in numbers)
        {
            if (num % 7 == 0)
            {
                count++;
            }
        }

        Console.WriteLine("Total numbers divisible by 7: " + count);
    }
    public static void Main(string[] args)
    {
        //Task - Datatypes
        //WelcomeUser();
        //PrintLargest();
        //Calculator();
        //ValidCredential();
        //CountDivisibleBy7();

        //Task - LoopAndControlStatement
        LoopAndControlState lc = new LoopAndControlState();
        //lc.FreqCounter();
        //lc.RotateArray(new int[] { 1, 2, 3, 4, 5});
        //lc.mergeTwoArray();
        //lc.GuessGame();
        //lc.validSudokuRow();
        //lc.validsudokuBoard();
        lc.cipherMsg();


    }
}