using Assembler.Grammar.AST.Operands;
using DCPU16;

namespace Assembler.Grammar.AST.Instructions
{
    public class SpecialInstruction
        : BaseInstruction
    {
        public SpecialOpcode Opcode { get; }
        public BaseOperand A { get; }

        public override uint WordLength => 1 + A.WordLength;

        public SpecialInstruction(SpecialOpcode opcode, BaseOperand a)
        {
            Opcode = opcode;
            A = a;
        }

        public override IEnumerable<ushort> Emit(IReadOnlyDictionary<string, ushort> map)
        {
            var instr = new Instruction(Opcode, A.Operand(true));
            yield return instr.Value;

            var aw = A.NextWord(map);
            if (aw.HasValue)
                yield return aw.Value;
        }
    }
}
