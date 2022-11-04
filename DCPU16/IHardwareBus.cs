namespace DCPU16
{
    public interface IHardwareBus
    {
        /// <summary>
        /// Get the number of connected devices
        /// </summary>
        ushort DeviceCount { get; }

        /// <summary>
        /// Get information about the device at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Device GetDevice(ushort index);

        /// <summary>
        /// Raise an interrupt with the given hardware device
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Extra cycles the CPU is frozen for while hardware handles the interrupt</returns>
        public byte Interrupt(ushort index, ref MachineState state);
    }

    public readonly struct Device
    {
        public readonly uint DeviceId;
        public readonly ushort Version;
        public readonly uint ManufacturerId;

        public Device(uint deviceId, ushort version, uint manufacturerId)
        {
            DeviceId = deviceId;
            Version = version;
            ManufacturerId = manufacturerId;
        }
    }
}
