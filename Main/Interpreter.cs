using System;
using System.Collections.Generic;

namespace SimpScript
{
    public class Interpreter
    {
        private Dictionary<string, string> _variables = new Dictionary<string, string>();

        public void Interpret(AstNode ast)
        {
            if (ast is ProgramNode programNode)
            {
                foreach (var statement in programNode.Statements)
                {
                    InterpretStatement(statement);
                }
            }
        }

        private void InterpretStatement(AstNode node)
        {
            if (node is PrintNode printNode)
            {
                InterpretPrint(printNode);
            }
            else if (node is AssignmentNode assignmentNode)
            {
                InterpretAssignment(assignmentNode);
            }
        }

        private void InterpretPrint(PrintNode printNode)
        {
            if (printNode.Value is StringConcatenationNode concatNode)
            {
                string leftValue = InterpretExpression(concatNode.Left);
                string rightValue = InterpretExpression(concatNode.Right);
                Console.WriteLine(leftValue + rightValue);
            }
            else
            {
                string value = InterpretExpression(printNode.Value);
                Console.WriteLine(value);
            }
        }

        private void InterpretAssignment(AssignmentNode assignmentNode)
        {
            string value = InterpretExpression(assignmentNode.Value);
            _variables[assignmentNode.Variable] = value;
        }

        private string InterpretExpression(AstNode node)
        {
            if (node is StringNode stringNode)
            {
                return stringNode.Value;
            }
            else if (node is IdentifierNode identifierNode)
            {
                if (_variables.ContainsKey(identifierNode.Name))
                {
                    return _variables[identifierNode.Name];
                }
                throw new Exception($"Undefined variable: {identifierNode.Name}");
            }
            else if (node is NumberNode numberNode)
            {
                return numberNode.Value;
            }

            throw new Exception($"Unknown expression type: {node.GetType().Name}");
        }
    }
}
