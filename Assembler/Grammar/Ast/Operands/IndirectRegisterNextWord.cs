using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    public class IndirectRegisterNextWord
        : BaseOperand
    {
        public Register Register { get; }
        public Number Value { get; }

        public override uint WordLength => 1;

        public IndirectRegisterNextWord(Register register, Number value)
        {
            Register = register;
            Value = value;
        }

        public override Operand Operand(bool a)
        {
            return DCPU16.Operand.INA + (int)Register;
        }

        public override ushort? NextWord(IReadOnlyDictionary<string, ushort> map)
        {
            return Value.Resolve(map);
        }
    }
}
