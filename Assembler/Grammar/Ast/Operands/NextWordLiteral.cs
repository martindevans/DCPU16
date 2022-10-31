using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class NextWordLiteral
        : BaseOperand
    {
        public Number Value { get; }

        public override uint WordLength => 1;

        public NextWordLiteral(Number value)
        {
            Value = value;
        }

        public override Operand Operand(bool a)
        {
            return DCPU16.Operand.NextWord;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return Value.Resolve(map);
        }
    }
}
