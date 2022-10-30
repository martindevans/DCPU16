using Assembler.Grammar.Ast.Instructions;

namespace Assembler.Grammar.Ast
{
    internal class Line
    {
        public string Label { get; }
        public BaseInstruction Instruction { get; }

        public Line(string label, BaseInstruction instr)
        {
            Label = label;
            Instruction = instr;
        }
    }
}
