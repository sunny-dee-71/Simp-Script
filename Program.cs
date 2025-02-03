using System;
using System.IO;
using SimpScript;

namespace SimpScript
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the path to a .ss file.");
                return;
            }

            string filePath = args[0];

            // Check if file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }

            string code = File.ReadAllText(filePath); // Read file content

            Lexer lexer = new(code);
            List<Token> tokens = lexer.Tokenize();

            Parser parser = new(tokens);
            AstNode ast = parser.Parse();

            // Now interpret the AST
            Interpreter interpreter = new();
            interpreter.Interpret(ast);

            Console.WriteLine("Execution complete!");
            Console.ReadKey(); // Add this line to keep the console open until the user presses a key
        }
    }
}
