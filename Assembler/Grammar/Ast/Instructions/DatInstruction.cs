namespace Assembler.Grammar.AST.Instructions
{
    public class DatInstruction
        : BaseInstruction
    {
        public IReadOnlyList<Number> Numbers { get; }

        public override uint WordLength => (uint)Numbers.Count;

        public DatInstruction(IReadOnlyList<Number> numbers)
        {
            Numbers = numbers;
        }

        public DatInstruction(string characters)
            : this(ToNumbers(characters))
        {
        }

        private static IReadOnlyList<Number> ToNumbers(string characters)
        {
            return characters
                .Select(c => (ushort)c)
                .Select(n => new Number(n))
                .ToList();
        }

        public override IEnumerable<ushort> Emit(IReadOnlyDictionary<string, ushort> map)
        {
            return Numbers.Select(a => a.Resolve(map));
        }
    }
}
