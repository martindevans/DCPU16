using DCPU16;

namespace Assembler.Grammar.AST.Operands
{
    internal class IndirectRegisterNextWord
        : BaseOperand
    {
        public Register Register { get; }
        public int Value { get; }

        public IndirectRegisterNextWord(Register register, int value)
        {
            Register = register;
            Value = value;
        }
    }
}
