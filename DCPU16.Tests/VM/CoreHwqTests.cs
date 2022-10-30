using DCPU16.Tests.Devices;

namespace DCPU16.Tests.VM
{
    [TestClass]
    public class Playground
        : CoreFixture
    {
        public Playground()
            : base(
                new NothingDevice(1, 2, 3),
                new NothingDevice(4, 5, 6)
            )
        {

        }

        [TestMethod]
        public void HwqQueriesDevice()
        {
            Memory[0] = new Instruction(BasicOpcode.SET, (Operand)0x22, Operand.I);
            Memory[1] = new Instruction(SpecialOpcode.HWQ, Operand.I);

            Core.Step();
            Assert.AreEqual(1, Core.MachineState.I);

            Core.Step();
            Assert.AreEqual(1, Core.MachineState.A);
            Assert.AreEqual(0, Core.MachineState.B);
            Assert.AreEqual(2, Core.MachineState.C);
            Assert.AreEqual(3, Core.MachineState.X);
            Assert.AreEqual(0, Core.MachineState.Y);
        }
    }
}