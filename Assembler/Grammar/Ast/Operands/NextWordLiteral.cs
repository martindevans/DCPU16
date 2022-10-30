namespace Assembler.Grammar.AST.Operands
{
    public class NextWordLiteral
        : BaseOperand
    {
        public int Value { get; }

        public NextWordLiteral(int value)
        {
            Value = value;
        }
    }
}
