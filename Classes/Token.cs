namespace SimpScript
{
    public class Token
    {
        public enum Type
        {
            Keyword,
            Identifier,
            Number,
            String,
            Operator,
            EndOfFile
        }

        public Type TokenType { get; set; }
        public string Value { get; set; }

        public Token(Type tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }
    }
}
