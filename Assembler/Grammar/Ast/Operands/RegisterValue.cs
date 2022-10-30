namespace Assembler.Grammar.Ast.Operands
{
    internal class RegisterValue
        : BaseOperand
    {
        public DCPU16.Register Register { get; }

        public RegisterValue(DCPU16.Register register)
        {
            Register = register;
        }
    }
}
