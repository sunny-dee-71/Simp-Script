using System;
using System.Collections.Generic;

namespace SimpScript
{
    public class Lexer
    {
        private string _code;
        private int _position = 0;

        public Lexer(string code)
        {
            _code = code;
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            while (_position < _code.Length)
            {
                char currentChar = _code[_position];

                if (char.IsWhiteSpace(currentChar))
                {
                    _position++;
                    continue;
                }

                if (currentChar == '"') // String literal
                {
                    _position++;
                    string strValue = "";
                    while (_position < _code.Length && _code[_position] != '"')
                    {
                        strValue += _code[_position];
                        _position++;
                    }
                    _position++; // Consume closing quote
                    tokens.Add(new Token(Token.Type.String, strValue));
                    continue;
                }

                if (char.IsDigit(currentChar)) // Number
                {
                    string numberValue = "";
                    while (_position < _code.Length && char.IsDigit(_code[_position]))
                    {
                        numberValue += _code[_position];
                        _position++;
                    }
                    tokens.Add(new Token(Token.Type.Number, numberValue));
                    continue;
                }

                if (char.IsLetter(currentChar)) // Identifier or keyword
                {
                    string identifier = "";
                    while (_position < _code.Length && (char.IsLetterOrDigit(_code[_position]) || _code[_position] == '_'))
                    {
                        identifier += _code[_position];
                        _position++;
                    }

                    if (identifier == "let" || identifier == "print") // Keyword
                    {
                        tokens.Add(new Token(Token.Type.Keyword, identifier));
                    }
                    else
                    {
                        tokens.Add(new Token(Token.Type.Identifier, identifier));
                    }
                    continue;
                }

                if (currentChar == '+' || currentChar == '=')
                {
                    tokens.Add(new Token(Token.Type.Operator, currentChar.ToString()));
                    _position++;
                    continue;
                }

                throw new Exception($"Unexpected character: {currentChar}");
            }

            tokens.Add(new Token(Token.Type.EndOfFile, ""));
            return tokens;
        }
    }
}
