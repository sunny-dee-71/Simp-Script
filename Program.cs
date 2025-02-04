using System;
using System.Collections.Generic;
using System.IO;

namespace SimpScript
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a .ss file.");
                return;
            }

            string filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }

            string[] lines = File.ReadAllLines(filePath);

            Interpreter interpreter = new();
            interpreter.Run(lines);

            Console.WriteLine("\nExecution complete! Press Enter to exit.");
            Console.ReadLine();  // **Wait for user input before closing**
        }
    }
}
