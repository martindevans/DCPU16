namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreIfeTests
        : CoreFixture
    {
        [TestMethod]
        public void IfeTrueLiterals()
        {
            Memory[0] = new Instruction(BasicOpcode.SET, (Operand)0x3f, Operand.I);
            Memory[1] = new Instruction(BasicOpcode.IFE, (Operand)0x3f, Operand.I);

            Core.Step();
            Core.Step();

            Assert.IsFalse(Core.MachineState.Skipping);
            Assert.AreEqual(2, Core.MachineState.PC);
        }

        [TestMethod]
        public void IfeFalseLiterals()
        {
            Memory[0] = new Instruction(BasicOpcode.SET, (Operand)0x3e, Operand.I);
            Memory[1] = new Instruction(BasicOpcode.IFE, (Operand)0x3f, Operand.I);

            Core.Step();
            Core.Step();

            Assert.IsTrue(Core.MachineState.Skipping);
            Assert.AreEqual(2, Core.MachineState.PC);

            Core.Step();

            Assert.IsFalse(Core.MachineState.Skipping);
            Assert.AreEqual(3, Core.MachineState.PC);
        }
    }
}