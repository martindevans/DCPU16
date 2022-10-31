using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class RegisterValue
        : BaseOperand
    {
        public Register Register { get; }

        public RegisterValue(Register register)
        {
            Register = register;
        }

        public override uint WordLength => 0;

        public override Operand Operand(bool a)
        {
            return (Operand)Register;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return null;
        }
    }
}
