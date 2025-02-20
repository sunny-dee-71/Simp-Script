using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpScript
{
    public class Interpreter
    {
        private Dictionary<string, string> variables = new Dictionary<string, string>();

        public void Run(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                i = Execute(lines, i);
            }
        }

        private int Execute(string[] lines, int index)
        {
            string line = lines[index].Trim();
            if (string.IsNullOrWhiteSpace(line) || line == "end") return index;

            string[] parts = line.Split(' ', 2);
            string command = parts[0];

            if (command == "say")
            {
                Console.WriteLine(Evaluate(parts[1]));
            }
            else if (command == "set")
            {
                string[] assignment = parts[1].Split(new string[] { " = " }, StringSplitOptions.None);
                if (assignment.Length != 2)
                {
                    Console.WriteLine("Error: Invalid assignment.");
                    return index;
                }
                string varName = assignment[0].Trim();
                string expr = assignment[1].Trim();

                if (expr.StartsWith("input"))
                {
                    string prompt = expr.Substring("input".Length).Trim().Trim('"');
                    Console.Write(prompt + ": ");
                    variables[varName] = Console.ReadLine();
                }
                else
                {
                    variables[varName] = Evaluate(expr);
                }
            }
            else if (command == "if")
            {
                string condition = parts[1].Trim();
                bool conditionMet = EvaluateCondition(condition);
                if (!conditionMet)
                {
                    index = SkipBlock(lines, index);
                }
            }
            else if (command == "repeat")
            {
                string loopParams = parts[1].Trim();
                return HandleRepeat(lines, index, loopParams);
            }
            else
            {
                Console.WriteLine("Error: Unknown command " + command);
            }
            return index;
        }

        private bool EvaluateCondition(string condition)
        {
            string[] operators = new string[] { "==", "!=", "<", "<=", ">", ">=" };
            string op = operators.FirstOrDefault(condition.Contains);
            if (op == null) return false;

            string[] parts = condition.Split(new string[] { op }, StringSplitOptions.None);
            if (parts.Length != 2) return false;

            string left = Evaluate(parts[0].Trim());
            string right = Evaluate(parts[1].Trim());

            if (int.TryParse(left, out int leftNum) && int.TryParse(right, out int rightNum))
            {
                return op switch
                {
                    "==" => leftNum == rightNum,
                    "!=" => leftNum != rightNum,
                    "<" => leftNum < rightNum,
                    "<=" => leftNum <= rightNum,
                    ">" => leftNum > rightNum,
                    ">=" => leftNum >= rightNum,
                    _ => false,
                };
            }
            return false;
        }

        private int HandleRepeat(string[] lines, int index, string loopParams)
        {
            int startIndex = index;
            List<string> block = ExtractBlock(lines, ref index);

            if (loopParams.Contains("times"))
            {
                int times = int.Parse(Evaluate(loopParams.Replace("times", "").Trim()));
                for (int i = 0; i < times; i++) Run(block.ToArray());
            }
            else if (loopParams.StartsWith("while"))
            {
                string condition = loopParams.Replace("while", "").Trim();
                while (EvaluateCondition(condition)) Run(block.ToArray());
            }
            else if (loopParams.Contains("from") && loopParams.Contains("to"))
            {
                string[] parts = loopParams.Split(new string[] { "from", "to" }, StringSplitOptions.None);
                string varName = parts[0].Trim();
                int start = int.Parse(Evaluate(parts[1].Trim()));
                int end = int.Parse(Evaluate(parts[2].Trim()));
                for (variables[varName] = start.ToString(); int.Parse(variables[varName]) <= end; variables[varName] = (int.Parse(variables[varName]) + 1).ToString())
                {
                    Run(block.ToArray());
                }
            }
            return index;
        }

        private List<string> ExtractBlock(string[] lines, ref int index)
        {
            List<string> block = new List<string>();
            index++;
            while (index < lines.Length && !lines[index].Trim().Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                block.Add(lines[index]);
                index++;
            }
            return block;
        }

        private int SkipBlock(string[] lines, int index)
        {
            index++;
            while (index < lines.Length && !lines[index].Trim().Equals("end", StringComparison.OrdinalIgnoreCase))
            {
                index++;
            }
            return index;
        }

        private string Evaluate(string expression)
        {
            foreach (var variable in variables)
            {
                expression = expression.Replace(variable.Key, variable.Value);
            }

            if (expression.Contains("+"))
            {
                string[] parts = expression.Split('+').Select(p => p.Trim()).ToArray();
                List<object> evaluatedParts = new List<object>();
                foreach (var part in parts)
                {
                    if (int.TryParse(part, out int number))
                        evaluatedParts.Add(number);
                    else if (part.StartsWith("\"") && part.EndsWith("\""))
                        evaluatedParts.Add(part.Trim('"'));
                    else
                        evaluatedParts.Add(part);
                }
                return evaluatedParts.All(p => p is int) ? evaluatedParts.Cast<int>().Sum().ToString() : string.Join("", evaluatedParts);
            }
            return expression.Trim('"');
        }
    }
}
