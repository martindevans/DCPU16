namespace Assembler.Grammar.Ast.Operands
{
    public class PushPop
        : BaseOperand
    {
        public bool Push { get; }

        public PushPop(bool push)
        {
            Push = push;
        }
    }
}
