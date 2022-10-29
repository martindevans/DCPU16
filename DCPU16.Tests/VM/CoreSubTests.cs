namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreSubTests
        : CoreFixture
    {
        [TestMethod]
        public void SubRegistersNoOverflow()
        {
            Core.MachineState.A = 5;
            Core.MachineState.B = 20;
            Memory[0] = new Instruction(BasicOpcode.SUB, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(5, Core.MachineState.A);
            Assert.AreEqual(15, Core.MachineState.B);
            Assert.AreEqual(1, Core.MachineState.PC);
            Assert.AreEqual(0, Core.MachineState.EX);
        }

        [TestMethod]
        public void SubRegistersOverflow()
        {
            Core.MachineState.A = 20;
            Core.MachineState.B = 10;
            Memory[0] = new Instruction(BasicOpcode.SUB, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(20, Core.MachineState.A);
            Assert.AreEqual(ushort.MaxValue - 9, Core.MachineState.B);
            Assert.AreEqual(1, Core.MachineState.PC);
            Assert.AreEqual(0xffff, Core.MachineState.EX);
        }
    }
}