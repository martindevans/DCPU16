using Assembler.Grammar.AST.Instructions;

namespace Assembler.Grammar.AST
{
    public class Line
    {
        public string? Label { get; }
        public BaseInstruction? Instruction { get; }

        public Line(string? label, BaseInstruction? instr)
        {
            Label = label;
            Instruction = instr;
        }
    }
}
