using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class IndirectNextWord
        : BaseOperand
    {
        public Number Value { get; }

        public override uint WordLength => 1;

        public IndirectNextWord(Number value)
        {
            Value = value;
        }

        public override Operand Operand(bool a)
        {
            return DCPU16.Operand.IndirectNextWord;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return Value.Resolve(map);
        }
    }
}
