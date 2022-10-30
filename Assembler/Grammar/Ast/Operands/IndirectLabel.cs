using Assembler.Grammar.AST.Operands;

namespace Assembler.Grammar.AST.Operands
{
    public class IndirectLabel
        : BaseOperand
    {
        public string Value { get; }

        public IndirectLabel(string value)
        {
            Value = value;
        }
    }
}
