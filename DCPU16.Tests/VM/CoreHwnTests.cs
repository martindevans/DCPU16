using DCPU16.Tests.Devices;

namespace DCPU16.Tests.VM
{
    [TestClass]
    public class CoreHwnTests
        : CoreFixture
    {
        public CoreHwnTests()
            : base(
                new NothingDevice(1, 2, 3)
            )
        {

        }

        [TestMethod]
        public void HwnQueriesDeviceCount()
        {
            Memory[0] = new Instruction(SpecialOpcode.HWN, Operand.J);
            Core.Step();

            Assert.AreEqual(1, Core.MachineState.J);
            Assert.AreEqual(1, Core.MachineState.PC);
            Assert.AreEqual(0, Core.MachineState.EX);
        }
    }
}