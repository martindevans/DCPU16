namespace Assembler.Grammar.Ast.Operands
{
    internal class IndirectNextWord
        : BaseOperand
    {
        public int Value { get; }

        public IndirectNextWord(int value)
        {
            Value = value;
        }
    }
}
