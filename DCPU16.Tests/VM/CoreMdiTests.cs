namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreMdiTests
        : CoreFixture
    {
        [TestMethod]
        public void MdiRegistersNoOverflow()
        {
            Core.MachineState.A = unchecked((ushort)-7);
            Core.MachineState.B = unchecked(16);
            Memory[0] = new Instruction(BasicOpcode.MDI, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(unchecked((ushort)-7), Core.MachineState.A);
            Assert.AreEqual(unchecked((ushort)-7), Core.MachineState.B);
            Assert.AreEqual(1, Core.MachineState.PC);
            Assert.AreEqual(0, Core.MachineState.EX);
        }
    }
}