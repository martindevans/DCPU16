using Assembler.Grammar.Ast.Operands;
using DCPU16;

namespace Assembler.Grammar.Ast.Instructions
{
    public class BasicInstruction
        : BaseInstruction
    {
        public BasicOpcode Opcode { get; }
        public BaseOperand A { get; }
        public BaseOperand B { get; }

        public BasicInstruction(BasicOpcode opcode, BaseOperand a, BaseOperand b)
        {
            Opcode = opcode;
            A = a;
            B = b;
        }
    }
}
