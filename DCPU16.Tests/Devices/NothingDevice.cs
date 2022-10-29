namespace DCPU16.Tests.Devices
{
    internal class NothingDevice
        : IHardwareDevice
    {
        private readonly uint _device;
        private readonly ushort _version;
        private readonly uint _manufacturer;

        public NothingDevice(uint device, ushort version, uint manufacturer)
        {
            _device = device;
            _version = version;
            _manufacturer = manufacturer;
        }

        public Device Query()
        {
            return new Device(
                _device,
                _version,
                _manufacturer
            );
        }

        public byte Interrupt()
        {
            return 1;
        }
    }
}
