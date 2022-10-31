using DCPU16;

namespace Assembler.Grammar.AST.Instructions
{
    public class ParameterlessSpecialInstruction
        : BaseInstruction
    {
        public SpecialOpcode Opcode { get; }

        public override uint WordLength => 1;

        public ParameterlessSpecialInstruction(SpecialOpcode opcode)
        {
            Opcode = opcode;
        }

        public override IEnumerable<ushort> Emit(IReadOnlyDictionary<string, ushort> map)
        {
            var instr = new Instruction(Opcode, Operand.A);
            yield return instr.Value;
        }
    }
}
