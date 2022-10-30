namespace Assembler.Grammar.AST.Operands
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
