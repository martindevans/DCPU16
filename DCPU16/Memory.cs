namespace DCPU16
{
    public class Memory
    {
        private readonly ushort[] _memory;
        public ref ushort this[ushort addr] => ref _memory[addr];

        public Memory()
        {
            _memory = new ushort[ushort.MaxValue];
        }
    }
}
