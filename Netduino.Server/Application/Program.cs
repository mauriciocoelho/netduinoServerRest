using System;
using Microsoft.SPOT;

namespace Netduino.Server.Application
{
    public class Program
    {
        private static Application _application;

        public static void Main()
        {
            _application = new Application();
            try
            {
                _application.Init();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
            }       
        }
    }
}
