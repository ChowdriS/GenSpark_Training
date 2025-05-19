using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace May19Morning
{
    internal class LoopAndControlState
    {
        public int getUserInput()
        {
            int val;
            while (!int.TryParse(Console.ReadLine(),out val) )
            {
                Console.WriteLine("Please enter a valid number...");
            }
            return val;
        }

        //Given an array, count the frequency of each element and print the result.
        public void FreqCounter()
        {
            int[] arr = new int[5];
            Dictionary<int,int> freqMap = new Dictionary<int,int>();
            for(int i=0; i<5; i++)
            {
                Console.Write($"Enter the number {i+1} : ");
                arr[i] = getUserInput();
            }

            Console.Write("Given Array => ");
            PrintArray(arr);

            Console.WriteLine("Frequency Count =>");
            foreach (var iter in arr)
            {
                if (freqMap.ContainsKey(iter))
                {
                    freqMap[iter]++;
                }
                else
                {
                    freqMap[iter] = 1;   
                }
            }
            foreach(var iter in freqMap)
            {
                Console.WriteLine($"{iter.Key} - {iter.Value}");
            }
        }

        public void PrintArray(int[] arr)
        {
            Console.WriteLine(string.Join(" ",arr));
        }

        //create a program to rotate the array to the left by one position.
        public void RotateArray(int[] arr)
        {
            Console.WriteLine("Before Rotation");
            PrintArray(arr);

            int firstElement = arr[0];
            int n = arr.Length;
            for (int i=1; i<n; i++)
            {
                arr[i - 1] = arr[i];
            }
            arr[n-1] = firstElement;

            Console.WriteLine("After Rotation");
            PrintArray(arr);
        }

        public int[] GetIntArrayFromuser()
        {
            Console.Write("Size of array - ");
            int ArrSize = getUserInput();

            int[] arr = new int[ArrSize];
            for (int i = 0; i < ArrSize; i++)
            {
                Console.Write($"Enter the number {i + 1} : ");
                arr[i] = getUserInput();
            }
            return arr;
        }
        //Given two integer arrays, merge them into a single array.
        public void mergeTwoArray()
        {
            Console.WriteLine("Get arr1 =>");
            int[] arr1 = GetIntArrayFromuser();
            Console.WriteLine("Get arr2 =>");
            int[] arr2 = GetIntArrayFromuser();

            Console.Write("Arr 1 => ");
            PrintArray(arr1);
            Console.Write("Arr 2 => ");
            PrintArray(arr2);

            int n1 = arr1.Length;
            int n2 = arr2.Length;
            int[] mergedArray = new int[n1 + n2];

            for (int i = 0; i < n1; i++)
            {
                mergedArray[i] = arr1[i];
            }
            for (int i = 0; i < n2; i++)
            {
                mergedArray[n1+i] = arr2[i];
            }

            Console.Write("Merged Array => ");
            PrintArray(mergedArray);
        }
    }
}
