namespace Assembler.Grammar.AST.Operands
{
    internal class IndirectRegister
        : BaseOperand
    {
        public DCPU16.Register Register { get; }

        public IndirectRegister(DCPU16.Register register)
        {
            Register = register;
        }
    }
}
