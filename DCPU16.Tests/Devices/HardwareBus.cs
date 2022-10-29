namespace DCPU16.Tests.Devices
{
    public class HardwareBus
        : IHardwareBus
    {
        private readonly IReadOnlyList<IHardwareDevice> _devices;

        public ushort DeviceCount => (ushort)_devices.Count;

        public HardwareBus(IReadOnlyCollection<IHardwareDevice> devices)
        {
            var d = new List<IHardwareDevice>(devices.Count);
            d.AddRange(devices);
            _devices = d;
        }

        public HardwareBus(params IHardwareDevice[] devices)
        {
            var d = new List<IHardwareDevice>(devices.Length);
            d.AddRange(devices);
            _devices = d;
        }

        public Device GetDevice(ushort index)
        {
            if (index >= _devices.Count)
                return default;

            return _devices[index].Query();
        }

        public byte Interrupt(ushort index)
        {
            if (index >= _devices.Count)
                return 0;

            return _devices[index].Interrupt();
        }
    }

    public interface IHardwareDevice
    {
        Device Query();

        byte Interrupt();
    }
}
