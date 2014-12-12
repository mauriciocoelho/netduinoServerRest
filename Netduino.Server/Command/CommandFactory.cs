using System;
using Microsoft.SPOT;

namespace Netduino.Server.Command
{
    public class CommandFactory
    {
        public static ICommand CreateCommand(bool state)
        {
            switch (state)
            {
                case true :
                    return new OnCommand();
                case false:
                    return new OffCommand();
                default:
                    return null;
            }
        }
    }
}
