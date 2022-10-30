namespace Assembler.Grammar.AST.Instructions
{
    public class DatInstruction
        : BaseInstruction
    {
        public IReadOnlyList<int> Numbers { get; }

        public DatInstruction(IReadOnlyList<int> numbers)
        {
            Numbers = numbers;
        }
    }
}
