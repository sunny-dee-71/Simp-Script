using System;
using System.Collections.Generic;

namespace SimpScript
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _position = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private Token Peek() => _position < _tokens.Count ? _tokens[_position] : new Token(Token.Type.EndOfFile, "");
        private Token Advance() => _tokens[_position++];

        public AstNode Parse()
        {
            List<AstNode> statements = new List<AstNode>();

            while (Peek().TokenType != Token.Type.EndOfFile)
            {
                statements.Add(ParseStatement());
            }

            return new ProgramNode(statements);
        }

        private AstNode ParseStatement()
        {
            Token token = Peek();

            if (token.TokenType == Token.Type.Keyword && token.Value == "print")
                return ParsePrint();

            if (token.TokenType == Token.Type.Keyword && token.Value == "let")
                return ParseAssignment();

            throw new Exception($"Unexpected token: {token.Value}");
        }

        private AstNode ParsePrint()
        {
            Advance(); // Consume 'print'
            Token valueToken = Advance(); // Get the value (string or variable)

            if (valueToken.TokenType != Token.Type.String && valueToken.TokenType != Token.Type.Identifier)
                throw new Exception("Expected string or variable after print");

            if (Peek().TokenType == Token.Type.Operator && Peek().Value == "+")
            {
                Advance(); // Consume '+'
                Token rightToken = Advance();
                if (rightToken.TokenType != Token.Type.Identifier && rightToken.TokenType != Token.Type.Number)
                    throw new Exception("Expected variable or number after '+'");

                return new PrintNode(new StringConcatenationNode(new StringNode(valueToken.Value), new IdentifierNode(rightToken.Value)));
            }

            return new PrintNode(new StringNode(valueToken.Value)); // Just print the value directly
        }

        private AstNode ParseAssignment()
        {
            Advance(); // Consume 'let'

            Token variableToken = Advance();
            if (variableToken.TokenType != Token.Type.Identifier)
                throw new Exception("Expected variable name after 'let'");

            if (Advance().Value != "=")
                throw new Exception("Expected '=' after variable name");

            Token valueToken = Advance();
            if (valueToken.TokenType != Token.Type.Number)
                throw new Exception("Expected number after '='");

            return new AssignmentNode(variableToken.Value, new NumberNode(valueToken.Value));
        }
    }
}
