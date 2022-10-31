using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public abstract class BaseOperand
    {
        public abstract uint WordLength { get; }

        public abstract Operand Operand(bool a);

        public abstract ushort? NextWord(IReadOnlyDictionary<string, ushort> map);
    }
}
