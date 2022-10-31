using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class PushPop
        : BaseOperand
    {
        public bool Push { get; }

        public override uint WordLength => 0;

        public PushPop(bool push)
        {
            Push = push;
        }

        public override Operand Operand(bool a)
        {
            if (a && Push)
                throw new InvalidOperationException("Cannot PUSH in operand B");
            if (!a && !Push)
                throw new InvalidOperationException("Cannot POP in operand B");

            return DCPU16.Operand.PushPop;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return null;
        }
    }
}
