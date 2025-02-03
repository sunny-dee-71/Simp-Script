namespace SimpScript
{
    public abstract class AstNode
    {
    }

    public class ProgramNode : AstNode
    {
        public List<AstNode> Statements { get; }

        public ProgramNode(List<AstNode> statements)
        {
            Statements = statements;
        }
    }

    public class PrintNode : AstNode
    {
        public AstNode Value { get; }

        public PrintNode(AstNode value)
        {
            Value = value;
        }
    }

    public class StringConcatenationNode : AstNode
    {
        public AstNode Left { get; }
        public AstNode Right { get; }

        public StringConcatenationNode(AstNode left, AstNode right)
        {
            Left = left;
            Right = right;
        }
    }

    public class AssignmentNode : AstNode
    {
        public string Variable { get; }
        public AstNode Value { get; }

        public AssignmentNode(string variable, AstNode value)
        {
            Variable = variable;
            Value = value;
        }
    }

    public class IdentifierNode : AstNode
    {
        public string Name { get; }

        public IdentifierNode(string name)
        {
            Name = name;
        }
    }

    public class StringNode : AstNode
    {
        public string Value { get; }

        public StringNode(string value)
        {
            Value = value;
        }
    }

    public class NumberNode : AstNode
    {
        public string Value { get; }

        public NumberNode(string value)
        {
            Value = value;
        }
    }
}
