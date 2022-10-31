using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class SmallLiteral
        : BaseOperand
    {
        public int Value { get; }

        public SmallLiteral(int value)
        {
            Value = value;
        }

        public override uint WordLength => 0;

        public override Operand Operand(bool a)
        {
            if (!a)
                throw new InvalidOperationException("Cannot emit small-literal for operand B");

            if (Value < -1)
                throw new InvalidOperationException("Small literal is too small");
            if (Value > 30)
                throw new InvalidOperationException("Small literal is too large");

            var v = unchecked((ushort)Value);
            return (Operand)v;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return null;
        }
    }
}
