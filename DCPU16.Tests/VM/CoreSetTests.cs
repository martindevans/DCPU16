namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreSetTests
        : CoreFixture
    {
        [TestMethod]
        public void CopyRegisterAB()
        {
            Core.MachineState.A = 10;
            Core.MachineState.B = 20;
            Memory[0] = new Instruction(BasicOpcode.SET, Operand.A, Operand.B);
            Core.Step();

            Assert.AreEqual(10, Core.MachineState.A);
            Assert.AreEqual(10, Core.MachineState.B);
            Assert.AreEqual(1, Core.MachineState.PC);
        }

        [TestMethod]
        public void CopyRegisterAC()
        {
            Core.MachineState.A = 10;
            Core.MachineState.B = 20;
            Core.MachineState.C = 30;
            Memory[0] = new Instruction(BasicOpcode.SET, Operand.A, Operand.C);
            Core.Step();

            Assert.AreEqual(10, Core.MachineState.A);
            Assert.AreEqual(20, Core.MachineState.B);
            Assert.AreEqual(10, Core.MachineState.C);
            Assert.AreEqual(1, Core.MachineState.PC);
        }
    }
}