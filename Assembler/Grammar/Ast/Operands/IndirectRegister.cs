using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class IndirectRegister
        : BaseOperand
    {
        public Register Register { get; }

        public override uint WordLength => 0;

        public IndirectRegister(Register register)
        {
            Register = register;
        }

        public override Operand Operand(bool a)
        {
            return DCPU16.Operand.IA + (int)Register;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return null;
        }
    }
}
