using Assembler.Grammar.AST.Operands;

namespace Assembler.Grammar.AST.Operands
{
    public class LabelValue
        : BaseOperand
    {
        public string Value { get; }

        public LabelValue(string value)
        {
            Value = value;
        }
    }
}
