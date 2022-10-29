namespace DCPU16
{
    // https://raw.githubusercontent.com/gatesphere/demi-16/master/docs/dcpu-specs/dcpu-1-7.txt

    public struct Instruction
    {
        public readonly ushort Value;

        public Instruction(BasicOpcode opcode, Operand a, Operand b)
        {
            Value = (ushort)((int)opcode | (int)b << 5 | (int)a << 10);
        }

        public Instruction(SpecialOpcode opcode, Operand a)
            : this(BasicOpcode.Special, a, (Operand)opcode)
        {
        }

        public static implicit operator ushort(Instruction i)
        {
            return i.Value;
        }
    }

    /// <summary>
    /// First 5 bits of a 16 bit instruction
    /// </summary>
    public enum BasicOpcode
    {
        Special = 0,

        SET = 1,
        ADD = 2,
        SUB = 3,
        MUL = 4,
        MLI = 5,
        DIV = 6,
        DVI = 7,
        MOD = 8,
        MDI = 9,
        AND = 10,
        BOR = 11,
        XOR = 12,
        SHR = 13,
        ASR = 14,
        SHL = 15,
        IFB = 16,
        IFC = 17,
        IFE = 18,
        IFN = 19,
        IFG = 20,
        IFA = 21,
        IFL = 22,
        IFU = 23,

        ADX = 26,
        SBX = 27,

        STI = 30,
        STD = 31
    }

    public enum SpecialOpcode
    {
        Reserved = 0,

        JSR = 1,

        HCF = 7,

        INT = 8,
        IAG = 9,
        IAS = 10,
        RFI = 11,
        IAQ = 12,

        HWN = 16,
        HWQ = 17,
        HWI = 18
    }

    public enum Operand
    {
        // Register
        A, B, C, X, Y, Z, I, J,

        // [Register]
        IA, IB, IC, IX, IY, IZ, II, IJ,

        // [Register + Next Word]
        INA, INB, INC, INX, INY, INZ, INI, INJ,

        PushPop,
        Peek,
        Pick,

        SP,
        PC,
        EX,

        // [Next Word]
        INW,

        // Next Word
        NW,

        // 0x20-0x3f : literal value 0xffff-0x1e (-1..30) (literal) (only for a)
    }
}
