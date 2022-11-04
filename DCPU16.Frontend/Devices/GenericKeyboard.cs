using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.OpenGLBinding;

namespace DCPU16.Frontend.Devices
{
    /// <summary>
    /// https://github.com/hydralisk98/dcpu-specs/blob/master/keyboard.txt
    /// </summary>
    internal class GenericKeyboard
        : IHardwareDevice
    {
        public Device Query()
        {
            return new Device(818902022u, 1, 0);
        }

        public byte Interrupt(ref MachineState state)
        {
            switch (state.A)
            {
                case 0:
                    ClearBuffer();
                    return 1;

                case 1:
                    state.C = ReadBuffer();
                    return 1;

                case 2:
                    state.C = IsPressed(state.B);
                    return 1;

                case 3 when state.B == 0:
                    DisableInterrupts();
                    return 1;

                case 3 when state.B != 0:
                    EnableInterrupts(state.B);
                    return 1;
            }

            return 0;
        }

        private void EnableInterrupts(ushort message)
        {
            throw new NotImplementedException();
        }

        private void DisableInterrupts()
        {
            throw new NotImplementedException();
        }

        private ushort IsPressed(ushort keycode)
        {
            throw new NotImplementedException();
        }

        private ushort ReadBuffer()
        {
            throw new NotImplementedException();
        }

        private void ClearBuffer()
        {
            throw new NotImplementedException();
        }

        public void AddToBuffer(Key key)
        {
            throw new NotImplementedException();
            //var code = (key) switch
            //{

            //};
        }
    }
}
