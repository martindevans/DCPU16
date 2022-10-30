namespace Assembler.Grammar.AST.Operands
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
