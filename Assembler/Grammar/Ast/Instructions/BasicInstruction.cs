using Assembler.Grammar.AST.Operands;
using DCPU16;

namespace Assembler.Grammar.AST.Instructions
{
    public class BasicInstruction
        : BaseInstruction
    {
        public BasicOpcode Opcode { get; }
        public BaseOperand A { get; }
        public BaseOperand B { get; }

        public override uint WordLength => 1 + A.WordLength + B.WordLength;

        public BasicInstruction(BasicOpcode opcode, BaseOperand a, BaseOperand b)
        {
            Opcode = opcode;
            A = a;
            B = b;
        }

        public override IEnumerable<ushort> Emit(IReadOnlyDictionary<string, ushort> map)
        {
            var instr = new Instruction(Opcode, A.Operand(true), B.Operand(false));
            yield return instr.Value;

            var aw = A.NextWord(map);
            if (aw.HasValue)
                yield return aw.Value;

            var bw = B.NextWord(map);
            if (bw.HasValue)
                yield return bw.Value;
        }
    }
}
