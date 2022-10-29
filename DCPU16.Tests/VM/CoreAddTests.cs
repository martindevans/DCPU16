namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreAddTests
        : CoreFixture
    {
        [TestMethod]
        public void AddRegistersNoOverflow()
        {
            Core.MachineState.A = 10;
            Core.MachineState.B = 20;
            Memory[0] = new Instruction(BasicOpcode.ADD, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(Core.MachineState.A, 10);
            Assert.AreEqual(Core.MachineState.B, 30);
            Assert.AreEqual(Core.MachineState.PC, 1);
            Assert.AreEqual(Core.MachineState.EX, 0);
        }

        [TestMethod]
        public void AddRegistersOverflow()
        {
            Core.MachineState.A = ushort.MaxValue;
            Core.MachineState.B = 20;
            Memory[0] = new Instruction(BasicOpcode.ADD, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(Core.MachineState.A, ushort.MaxValue);
            Assert.AreEqual(Core.MachineState.B, 19);
            Assert.AreEqual(Core.MachineState.PC, 1);
            Assert.AreEqual(Core.MachineState.EX, 1);
        }

        [TestMethod]
        public void AddRegisterLiteralNoOverflow()
        {
            Core.MachineState.B = 20;
            Memory[0] = new Instruction(BasicOpcode.ADD, (Operand)0x20, Operand.B);
            Core.Step();

            Assert.AreEqual(Core.MachineState.B, 19);
            Assert.AreEqual(Core.MachineState.PC, 1);
            Assert.AreEqual(Core.MachineState.EX, 1);
        }
    }
}