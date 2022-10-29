using DCPU16.Tests.Devices;

namespace DCPU16.Tests.VM
{
    public abstract class CoreFixture
    {
        protected Memory Memory;
        protected Core Core;
        protected IHardwareBus Bus;

        protected CoreFixture(params IHardwareDevice[] devices)
        {
            Bus = new HardwareBus(devices);
            Memory = new Memory();
            Core = new Core(Memory, Bus);
        }
    }
}
