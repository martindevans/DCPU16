namespace Assembler.Grammar.AST.Operands
{
    internal class NextWordLiteral
        : BaseOperand
    {
        public int Value { get; }

        public NextWordLiteral(int value)
        {
            Value = value;
        }
    }
}
