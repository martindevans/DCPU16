namespace Assembler.Grammar.Ast.Operands
{
    public class SmallLiteral
        : BaseOperand
    {
        public int Value { get; }

        public SmallLiteral(int value)
        {
            Value = value;
        }
    }
}
