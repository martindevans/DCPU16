namespace DCPU16
{
    public class Core
    {
        private readonly Memory _memory;
        private readonly IHardwareBus _bus;

        private MachineState _state;

        public ulong Cycles { get; private set; }

        public ref MachineState MachineState => ref _state;

        public Core(Memory memory, IHardwareBus bus, MachineState state = default)
        {
            _memory = memory;
            _state = state;
            _bus = bus;
        }
        
        private byte BasicOp(BasicOpcode opcode, byte opa, byte opb)
        {
            if (opcode == BasicOpcode.Special)
                return SpecialOp(opa, opb);

            ref var a = ref MachineState.ResolveOperand(ref _state, (Operand)opa, _memory, true);
            ref var b = ref MachineState.ResolveOperand(ref _state, (Operand)opb, _memory, false);

            switch (opcode)
            {
                case BasicOpcode.SET:
                {
                    b = a;
                    return 1;
                }

                case BasicOpcode.ADD:
                {
                    var sum = a + b;
                    _state.EX = (sum > ushort.MaxValue ? (ushort)1 : (ushort)0);
                    b = unchecked((ushort)sum);
                    return 2;
                }

                case BasicOpcode.SUB:
                {
                    var sub = b - a;
                    _state.EX = (sub < 0 ? (ushort)0xffff : (ushort)0);
                    b = unchecked((ushort)sub);
                    return 2;
                }

                case BasicOpcode.MUL:
                {
                    b = unchecked((ushort)(a * b));
                    _state.EX = unchecked((ushort)(((b * a) >> 16) & 0xffff));
                    return 2;
                }

                case BasicOpcode.MLI:
                    throw new NotImplementedException("ML1");

                case BasicOpcode.DIV:
                {
                    if (a == 0)
                    {
                        _state.B = 0;
                        _state.EX = 0;
                    }
                    else
                    {
                        b = unchecked((ushort)(b / a));
                        _state.EX = unchecked((ushort)(((b << 16) / a) & 0xffff));
                    }
                    return 3;
                }

                case BasicOpcode.DVI:
                    throw new NotImplementedException("DVI");

                case BasicOpcode.MOD:
                {
                    if (a == 0)
                        _state.B = 0;
                    else
                        b = unchecked((ushort)(b % a));
                    
                    return 3;
                }

                case BasicOpcode.MDI:
                {
                    if (a == 0)
                    {
                        _state.B = 0;
                    }
                    else
                    {
                        var aa = unchecked((short)a);
                        var bb = unchecked((short)b);
                        b = unchecked((ushort)(aa % bb));
                    }
                    return 3;
                }

                case BasicOpcode.AND:
                {
                    b = (ushort)(b & a);
                    return 1;
                }

                case BasicOpcode.BOR:
                {
                    b = (ushort)(b | a);
                    return 1;
                }

                case BasicOpcode.XOR:
                {
                    b = (ushort)(b ^ a);
                    return 1;
                }

                case BasicOpcode.SHR:
                    throw new NotImplementedException("SHR");

                case BasicOpcode.ASR:
                    throw new NotImplementedException("ASR");

                case BasicOpcode.SHL:
                {
                    _state.B = unchecked((ushort)(b << a));
                    _state.EX = unchecked((ushort)(((b << a) >> 16) & 0xffff));
                    return 1;
                }

                case BasicOpcode.IFB:
                {
                    return If((b & a) != 0);
                }

                case BasicOpcode.IFC:
                {
                    return If((b & a) == 0);
                }

                case BasicOpcode.IFE:
                {
                    return If(b == a);
                }

                case BasicOpcode.IFN:
                {
                    return If(b != a);
                }

                case BasicOpcode.IFG:
                {
                    return If(b > a);
                }

                case BasicOpcode.IFA:
                {
                    var sa = unchecked((short)a);
                    var sb = unchecked((short)b);
                    return If(sb > sa);
                }

                case BasicOpcode.IFL:
                {
                    return If(b < a);
                }

                case BasicOpcode.IFU:
                {
                    var sa = unchecked((short)a);
                    var sb = unchecked((short)b);
                    return If(sb < sa);
                }

                case BasicOpcode.ADX:
                    throw new NotImplementedException("ADX");

                case BasicOpcode.SBX:
                    throw new NotImplementedException("SBX");

                case BasicOpcode.STI:
                {
                    b = a;
                    _state.I++;
                    _state.J++;
                    return 2;
                }

                case BasicOpcode.STD:
                {
                    b = a;
                    _state.I--;
                    _state.J--;
                    return 2;
                }

                default:
                {
                    _state.OnFire = true;
                    return 1;
                }
            }
        }

        private byte If(bool condition)
        {
            if (!condition)
                _state.Skipping = true;
            return 2;
        }

        private byte SpecialOp(byte opa, byte opb)
        {
            ref var a = ref MachineState.ResolveOperand(ref _state, (Operand)opa, _memory, true);

            switch ((SpecialOpcode)opb)
            {
                case SpecialOpcode.Reserved:
                {
                    return 1;
                }

                case SpecialOpcode.JSR:
                {
                    StackPush(_state.PC);
                    _state.PC = _state.A;
                    return 3;
                }

                case SpecialOpcode.INT:
                {
                    RaiseInterrupt(a);
                    return 4;
                }

                case SpecialOpcode.IAG:
                {
                    _state.A = _state.IA;
                    return 1;
                }

                case SpecialOpcode.IAS:
                {
                    _state.IA = _state.A;
                    return 1;
                }

                case SpecialOpcode.RFI:
                {
                    _state.InterruptQueueing = false;
                    _state.A = StackPop();
                    _state.PC = StackPop();
                    return 3;
                }

                case SpecialOpcode.IAQ:
                {
                    _state.InterruptQueueing = _state.A != 0;
                    return 2;
                }

                case SpecialOpcode.HWN:
                {
                    a = _bus.DeviceCount;
                    return 2;
                }

                case SpecialOpcode.HWQ:
                {
                    var d = _bus.GetDevice(_state.A);
                    _state.A = (ushort)(d.DeviceId & 0xFFFF);
                    _state.B = (ushort)((d.DeviceId >> 16) & 0xFFFF);
                    _state.C = d.Version;
                    _state.X = (ushort)(d.ManufacturerId & 0xFFFF);
                    _state.Y = (ushort)((d.ManufacturerId >> 16) & 0xFFFF);
                    return 4;
                }

                case SpecialOpcode.HWI:
                {
                    return (byte)((4 + _bus.Interrupt(_state.A)) & 0xFF);
                }

                default:
                {
                    _state.OnFire = true;
                    return 1;
                }
            }
        }
        
        public void Step()
        {
            if (_state.OnFire)
            {
                Cycles++;
                return;
            }

            var word = _memory[_state.PC++];
            var op  = (byte)(word & 0b11111);
            var b = (byte)((word >> 5)  & 0b011111);
            var a = (byte)((word >> 10) & 0b111111);

            if (_state.Skipping)
            {
                if (op is >= 0x10 and <= 0x17)
                {
                    Cycles++;
                    return;
                }
                else
                    _state.Skipping = false;
            }

            Cycles += BasicOp((BasicOpcode)op, a, b);

            if (_state.OnFire)
                return;

            if (!_state.InterruptQueueing)
            {
                var message = _state.Dequeue();
                if (message.HasValue)
                    ProcessInterrupt(message.Value);
            }
        }

        
        private ushort StackPop()
        {
            return _memory[_state.SP++];
        }

        private void StackPush(ushort value)
        {
            _memory[--_state.SP] = value;
        }


        private void RaiseInterrupt(ushort message)
        {
            if (_state.InterruptQueueing || _state.Skipping)
            {
                _state.Enqueue(message);
                return;
            }

            ProcessInterrupt(message);
        }

        private void ProcessInterrupt(ushort message)
        {
            if (_state.IA == 0)
                return;

            _state.InterruptQueueing = true;
            StackPush(_state.PC);
            StackPush(_state.A);
            _state.PC = _state.IA;
            _state.A = message;
        }
    }
}