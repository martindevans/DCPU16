namespace Assembler.Grammar.AST
{
    public class Number
    {
        private readonly string? _label;
        private readonly int? _value;

        public string? Label => _label;
        public int? Value => _value;

        public Number(int value)
        {
            _value = value;
            _label = null;
        }

        public Number(string label)
        {
            _value = null;
            _label = label;
        }

        public ushort Resolve(IReadOnlyDictionary<string, ushort> map)
        {
            if (_label != null)
                return map[_label];
            else
                return checked((ushort)_value!.Value);
        }
    }
}
