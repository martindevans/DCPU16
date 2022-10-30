using Assembler.Grammar.AST.Operands;
using DCPU16;

namespace Assembler.Grammar.AST.Instructions
{
    public class SpecialInstruction
        : BaseInstruction
    {
        public SpecialOpcode Opcode { get; }
        public BaseOperand A { get; }

        public SpecialInstruction(SpecialOpcode opcode, BaseOperand a)
        {
            Opcode = opcode;
            A = a;
        }
    }
}
