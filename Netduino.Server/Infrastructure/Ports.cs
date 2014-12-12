using System;
using Microsoft.SPOT.Hardware;
using Netduino.Server.Command;
using Netduino.Server.ValueObject;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace Netduino.Server.Infrastructure
{
    public class Ports
    {
        private static Ports _instance;
        private OutputPort[] _outputPorts;
        private readonly PortState _portState = new PortState();

        public static Ports Instance
        {
            get { return _instance ?? (_instance = new Ports()); }
        }

        private Ports()
        {
            SetupOutputPorts();
        }

        public OutputPort GetOutputPort(Int32 index)
        {
            return _outputPorts[index];
        }

        private void SetupOutputPorts()
        {
            _outputPorts = new OutputPort[14];
            _outputPorts[0] = new OutputPort(Pins.GPIO_PIN_D0, false);
            _outputPorts[1] = new OutputPort(Pins.GPIO_PIN_D1, false);
            _outputPorts[2] = new OutputPort(Pins.GPIO_PIN_D2, false);
            _outputPorts[3] = new OutputPort(Pins.GPIO_PIN_D3, false);
            _outputPorts[4] = new OutputPort(Pins.GPIO_PIN_D4, false);
            _outputPorts[5] = new OutputPort(Pins.GPIO_PIN_D5, false);
            _outputPorts[6] = new OutputPort(Pins.GPIO_PIN_D6, false);
            _outputPorts[7] = new OutputPort(Pins.GPIO_PIN_D7, false);
            _outputPorts[8] = new OutputPort(Pins.GPIO_PIN_D8, false);
            _outputPorts[9] = new OutputPort(Pins.GPIO_PIN_D9, false);
            _outputPorts[10] = new OutputPort(Pins.GPIO_PIN_D10, false);
            _outputPorts[11] = new OutputPort(Pins.GPIO_PIN_D11, false);
            _outputPorts[12] = new OutputPort(Pins.GPIO_PIN_D12, false);
            _outputPorts[13] = new OutputPort(Pins.GPIO_PIN_D13, false);
        }

        public class PortState
        {            
            public bool Port0State = false;
            public bool Port1State = false;
            public bool Port2State = false;
            public bool Port3State = false;
            public bool Port4State = false;
            public bool Port5State = false;
            public bool Port6State = false;
            public bool Port7State = false;
            public bool Port8State = false;
            public bool Port9State = false;
            public bool Port10State = false;
            public bool Port11State = false;
            public bool Port12State = false;
        }

        public void SetPort(PinReceiveBody pinReceiveBody)
        {
            ICommand command = CommandFactory.CreateCommand(pinReceiveBody.Status);
            command.Execute(pinReceiveBody.Pin);
            SetPortState(pinReceiveBody.Pin, pinReceiveBody.Status);
        }

        private void SetPortState(int port, bool state)
        {
            switch (port)
            {
                case 0:
                    _portState.Port0State = state;
                    break;
                case 1:
                    _portState.Port1State = state;
                    break;
                case 2:
                    _portState.Port2State = state;
                    break;
                case 3:
                    _portState.Port3State = state;
                    break;
                case 4:
                    _portState.Port4State = state;
                    break;
                case 5:
                    _portState.Port5State = state;
                    break;
                case 6:
                    _portState.Port6State = state;
                    break;
                case 7:
                    _portState.Port7State = state;
                    break;
                case 8:
                    _portState.Port8State = state;
                    break;
                case 9:
                    _portState.Port9State = state;
                    break;
                case 10:
                    _portState.Port10State = state;
                    break;
                case 11:
                    _portState.Port11State = state;
                    break;
                case 12:
                    _portState.Port12State = state;
                    break;
            }
        }

        public bool GetStatePort(Int32 index)
        {
            switch (index)
            {
                case 0:
                    return _portState.Port0State;
                case 1:
                    return _portState.Port1State;
                case 2:
                    return _portState.Port2State;
                case 3:
                    return _portState.Port3State;
                case 4:
                    return _portState.Port4State;
                case 5:
                    return _portState.Port5State;
                case 6:
                    return _portState.Port6State;
                case 7:
                    return _portState.Port7State;
                case 8:
                    return _portState.Port8State;
                case 9:
                    return _portState.Port9State;
                case 10:
                    return _portState.Port10State;
                case 11:
                    return _portState.Port11State;
                case 12:
                    return _portState.Port12State;
                default:
                    return false;
            }
        }


    }
}
