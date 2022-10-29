using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DCPU16
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MachineState
    {
        public ushort A;
        public ushort B;
        public ushort C;
        public ushort X;
        public ushort Y;
        public ushort Z;
        public ushort I;
        public ushort J;
        public ushort PC;
        public ushort EX;
        public ushort IA;
        public ushort SP;

        public bool InterruptQueueing;
        public bool OnFire;
        public bool Skipping;

        private unsafe fixed ushort _queue[256];
        private int _end;
        private int _start;
        private int _count;

        private ushort _literala;
        private ushort _literalb;

        internal static ref ushort ResolveOperand(ref MachineState state, Operand operand, Memory memory, bool a)
        {
            unchecked
            {
                switch (operand)
                {
                    case Operand.A: case Operand.B: case Operand.C:
                    case Operand.X: case Operand.Y: case Operand.Z:
                    case Operand.I: case Operand.J:
                        return ref BasicRegister(ref state, (int)operand);

                    case Operand.IA: case Operand.IB: case Operand.IC: 
                    case Operand.IX: case Operand.IY: case Operand.IZ: 
                    case Operand.II: case Operand.IJ:
                        return ref memory[BasicRegister(ref state, (int)operand & 7)];

                    case Operand.INA: case Operand.INB: case Operand.INC: 
                    case Operand.INX: case Operand.INY: case Operand.INZ: 
                    case Operand.INI: case Operand.INJ:
                        var nw = NextWord(ref state);
                        return ref memory[(ushort)(BasicRegister(ref state, (int)operand & 7) + nw)];

                    case Operand.PushPop:
                        return ref a
                            ? ref memory[state.SP++]
                            : ref memory[--state.SP];

                    case Operand.Peek: return ref memory[state.SP];
                    case Operand.Pick: return ref memory[(ushort)(state.SP + NextWord(ref state))];

                    case Operand.SP: return ref state.SP;
                    case Operand.PC: return ref state.PC;
                    case Operand.EX: return ref state.EX;

                    case Operand.INW: return ref memory[NextWord(ref state)];
                    case Operand.NW:  return ref NextWord(ref state);

                    default:
                    {
                        unchecked
                        {
                            var literal = (ushort)((ushort)operand - 0x21);

                            if (a)
                            {
                                state._literala = literal;
                                return ref state._literala;
                            }
                            else
                            {
                                state._literalb = literal;
                                return ref state._literalb;
                            }
                        }
                    }
                }
            }

            unsafe ref ushort BasicRegister(ref MachineState registers, int index)
            {
                var ptr = (ushort*)Unsafe.AsPointer(ref registers) + index;
                return ref Unsafe.AsRef<ushort>(ptr);
            }

            ref ushort NextWord(ref MachineState registers)
            {
                return ref memory[registers.PC++];
            }
        }

        internal bool Enqueue(ushort value)
        {
            if (_count >= 256)
                return false;

            unsafe
            {
                _queue[_end++] = value;
            }

            _count++;
            _end %= 256;
            return true;
        }

        internal ushort? Dequeue()
        {
            if (_count <= 0)
                return null;

            unsafe
            {
                var item = _queue[_start++];
                _start %= 256;
                _count--;
                return item;
            }
        }
    }
}
