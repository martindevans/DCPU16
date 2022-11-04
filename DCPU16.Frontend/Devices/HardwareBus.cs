using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCPU16.Frontend.Devices
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

        public byte Interrupt(ushort index, ref MachineState state)
        {
            if (index >= _devices.Count)
                return 0;

            return _devices[index].Interrupt(ref state);
        }
    }

    public interface IHardwareDevice
    {
        Device Query();

        byte Interrupt(ref MachineState state);
    }
}
