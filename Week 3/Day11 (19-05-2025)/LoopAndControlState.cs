using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace May19Morning
{
    internal class LoopAndControlState
    {
        public int getUserInput()
        {
            int val;
            while (!int.TryParse(Console.ReadLine(), out val))
            {
                Console.WriteLine("Please enter a valid number...");
            }
            return val;
        }

        //6. Given an array, count the frequency of each element and print the result.
        public void FreqCounter()
        {
            int[] arr = new int[5];
            Dictionary<int, int> freqMap = new Dictionary<int, int>();
            for (int i = 0; i < 5; i++)
            {
                Console.Write($"Enter the number {i + 1} : ");
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
            foreach (var iter in freqMap)
            {
                Console.WriteLine($"{iter.Key} - {iter.Value}");
            }
        }

        public void PrintArray(int[] arr)
        {
            Console.WriteLine(string.Join(" ", arr));
        }

        //7. create a program to rotate the array to the left by one position.
        public void RotateArray(int[] arr)
        {
            if(arr.Length == 0)
            {
                Console.WriteLine("Array is Empty");
                return;
            }

            Console.WriteLine("Before Rotation");
            PrintArray(arr);

            int firstElement = arr[0];
            int n = arr.Length;
            for (int i = 1; i < n; i++)
            {
                arr[i - 1] = arr[i];
            }
            arr[n - 1] = firstElement;

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
        //8. Given two integer arrays, merge them into a single array.
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
                mergedArray[n1 + i] = arr2[i];
            }

            Console.Write("Merged Array => ");
            PrintArray(mergedArray);
        }
        //9. Accepts user input as a 4-letter word guess.
        public void GuessGame()
        {
            string secret = "GAME";
            int attempts = 0;
            string? guess;

            while (true)
            {
                Console.Write("Enter your 4-letter guess: ");
                guess = Console.ReadLine();
                guess = guess.ToUpper();
                attempts++;

                if (guess.Length != 4)
                {
                    Console.WriteLine("Please enter exactly 4 letters.");
                    continue;
                }

                int bulls = 0;
                int cows = 0;
                bool[] secretCorrect = new bool[4];
                bool[] guessCorrect = new bool[4];

                for (int i = 0; i < 4; i++)
                {
                    if (guess[i] == secret[i])
                    {
                        bulls++;
                        secretCorrect[i] = true;
                        guessCorrect[i] = true;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (guessCorrect[i]) continue;

                    for (int j = 0; j < 4; j++)
                    {
                        if (!secretCorrect[j] && guess[i] == secret[j])
                        {
                            cows++;
                            secretCorrect[j] = true;
                            break;
                        }
                    }
                }

                Console.WriteLine($"{bulls} Bulls, {cows} Cows");

                if (bulls == 4)
                {
                    Console.WriteLine($"Congratulations! You guessed the word in {attempts} attempt(s).");
                    break;
                }
            }
        }

        //10) write a program that accepts a 9-element array representing a Sudoku row.
        public void validSudokuRow()
        {
            int[] arr = new int[9];
            Console.WriteLine("Enter the array =>");
            for(int i = 0; i < 9; i++)
            {
                Console.Write($"Enter the number {i + 1} : ");
                arr[i] = getUserInput();
            }

            Array.Sort(arr);
            bool valid = true;
            for (int i = 0; i<9; i++)
            {
                if (arr[i] != (i + 1))
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                Console.WriteLine("It is a valid row in sudoku!");
            }
            else
            {
                Console.WriteLine("It is not a valid row in sudoku!");
            }
        }
        public bool ValidateRows(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                HashSet<int> seen = new HashSet<int>();
                for (int col = 0; col < 9; col++)
                {
                    int num = board[row, col];
                    if (num < 1 || num > 9 || !seen.Add(num))
                    {
                        Console.WriteLine($"Invalid row at index {row + 1}");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateColumns(int[,] board)
        {
            for (int col = 0; col < 9; col++)
            {
                HashSet<int> seen = new HashSet<int>();
                for (int row = 0; row < 9; row++)
                {
                    int num = board[row, col];
                    if (num < 1 || num > 9 || !seen.Add(num))
                    {
                        Console.WriteLine($"Invalid column at index {col + 1}");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateBoxes(int[,] board)
        {
            for (int boxRow = 0; boxRow < 3; boxRow++)
            {
                for (int boxCol = 0; boxCol < 3; boxCol++)
                {
                    HashSet<int> seen = new HashSet<int>();

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int row = boxRow * 3 + i;
                            int col = boxCol * 3 + j;
                            int num = board[row, col];

                            if (num < 1 || num > 9 || !seen.Add(num))
                            {
                                Console.WriteLine($"Invalid box at ({boxRow + 1}, {boxCol + 1})");
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        //11. Validate whole sudoku board
        public void validsudokuBoard()
        {
            //int[,] board = new int[9, 9];

            int[,] board = new int[9, 9]
            {
                {5,3,4,6,7,8,9,1,2},
                {6,7,2,1,9,5,3,4,8},
                {1,9,8,3,4,2,5,6,7},
                {8,5,9,7,6,1,4,2,3},
                {4,2,6,8,5,3,7,9,1},
                {7,1,3,9,2,4,8,5,6},
                {9,6,1,5,3,7,2,8,4},
                {2,8,7,4,1,9,6,3,5},
                {3,4,5,2,8,6,1,7,9}
            };

            //for (int i = 0; i < 9; i++)
            //{
            //    for(int j = 0; j < 9; j++)
            //    {
            //        Console.Write($"Enter the number [{i}][{j}] : ");
            //        board[i,j] = getUserInput();
            //    }
            //}

            bool rowsValid = ValidateRows(board);
            bool colsValid = ValidateColumns(board);
            bool boxesValid = ValidateBoxes(board);

            if (rowsValid && colsValid && boxesValid)
                Console.WriteLine("The Sudoku board is valid!");
            else
                Console.WriteLine("Invalid Sudoku board detected.");
        }

        public string Encrypt(string? input, int shift)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char enc = (char)((c - 'a' + shift) % 26 + 'a');
                result[i] = enc;
            }
            return new string(result);
        }

        public string Decrypt(string input, int shift)
        {
            char[] result = new char[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                char dec = (char)((c - 'a' - shift + 26) % 26 + 'a');
                result[i] = dec;
            }
            return new string(result);
        }
        public void cipherMsg()
        {
            string? message;
            while (true)
            {
                Console.Write("Enter message (lowercase letters only): ");
                message = Console.ReadLine();

                if (Regex.IsMatch(message, "^[a-z]+$"))
                    break;
                else
                    Console.WriteLine("Invalid input! Only lowercase letters are allowed, no spaces or symbols.");
            }

            Console.Write("Enter shift amount: ");
            int shift = getUserInput();

            string encrypted = Encrypt(message, shift);
            string decrypted = Decrypt(encrypted, shift);

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");
        }
    }
}
