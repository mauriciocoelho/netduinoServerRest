using Microsoft.SPOT.Hardware;
using Netduino.Server.Infrastructure;

namespace Netduino.Server.Command
{
    public class OffCommand : ICommand
    {
        public void Execute(int portCommand)
        {
            OutputPort port = Ports.Instance.GetOutputPort(portCommand);

            port.Write(false);
        }
    }
}
