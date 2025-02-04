using System;
using System.Collections.Generic;
using System.Data;

namespace SimpScript
{
    public class Interpreter
    {
        // Stores variables as string values.
        private Dictionary<string, string> variables = new Dictionary<string, string>();

        // Run the interpreter with an array of code lines.
        public void Run(string[] lines)
        {
            foreach (string line in lines)
            {
                Execute(line.Trim());
            }
        }

        // Execute a single line.
        private void Execute(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return;

            // Split the line into command and the rest of the expression.
            string[] parts = line.Split(' ', 2);
            string command = parts[0];

            if (command == "say")
            {
                // Evaluate the expression after "print" and output the result.
                Console.WriteLine(Evaluate(parts[1]));
            }
            else if (command == "set")
            {
                // For variable assignment, the format is: let x = expression
                // We split on " = " (with spaces) for simplicity.
                string[] assignment = parts[1].Split(new string[] { " = " }, StringSplitOptions.None);
                if (assignment.Length != 2)
                {
                    Console.WriteLine("Error: Invalid assignment.");
                    return;
                }
                string varName = assignment[0].Trim();
                string expr = assignment[1].Trim();

                // Check if the assignment is for input, e.g., input "Enter your name"
                if (expr.StartsWith("input"))
                {
                    // Remove the "input" keyword
                    string prompt = expr.Substring("input".Length).Trim();
                    // Remove quotes from the prompt if they exist
                    prompt = prompt.Trim('"');
                    Console.Write(prompt + ": ");
                    string userInput = Console.ReadLine();
                    variables[varName] = userInput;
                }
                else
                {
                    variables[varName] = Evaluate(expr);
                }
            }
            else
            {
                Console.WriteLine("Error: Unknown command " + command);
            }
        }

        // Evaluate an expression (supports '+' operator for addition or concatenation).
        private string Evaluate(string expression)
        {
            // First, replace variable names with their values.
            foreach (var variable in variables)
            {
                // Use a simple replacement; in a robust solution, you'd parse tokens
                expression = expression.Replace(variable.Key, variable.Value);
            }

            // If the expression contains the '+' operator, we will split and process each part.
            if (expression.Contains("+"))
            {
                // Split on '+' and trim each part.
                string[] parts = expression.Split('+').Select(p => p.Trim()).ToArray();
                List<object> evaluatedParts = new List<object>();

                foreach (var part in parts)
                {
                    // If the part is numeric, treat it as an int.
                    if (int.TryParse(part, out int number))
                    {
                        evaluatedParts.Add(number);
                    }
                    // If the part is a quoted string, remove the quotes.
                    else if (part.StartsWith("\"") && part.EndsWith("\""))
                    {
                        evaluatedParts.Add(part.Trim('"'));
                    }
                    else
                    {
                        // Otherwise, leave it as a string.
                        evaluatedParts.Add(part);
                    }
                }

                // If every part is an int, sum them.
                if (evaluatedParts.All(p => p is int))
                {
                    int sum = evaluatedParts.Cast<int>().Sum();
                    return sum.ToString();
                }
                else
                {
                    // Otherwise, concatenate all parts as strings.
                    return string.Join("", evaluatedParts.Select(p => p.ToString()));
                }
            }

            // If there's no '+' operator, simply remove surrounding quotes (if any) and return.
            return expression.Trim('"');
        }
    }
}
