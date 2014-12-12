using System;
using Json.NETMF;
using Netduino.Server.Infrastructure;

namespace Netduino.Server.ValueObject
{

    public class AllPinsResponseBody
    {
        private static AllPinsResponseBody _instance;
        
        public Pins pins = new Pins();

        public static AllPinsResponseBody Instance
        {
            get { return _instance ?? (_instance = new AllPinsResponseBody()); }
        }

        internal string MounthResponsePinsJson()
        {

            var allPins = new AllPinsResponseBody
            {
                pins =
                {
                    Pin0 = Ports.Instance.GetStatePort(0).ToString(),
                    Pin1 = Ports.Instance.GetStatePort(1).ToString(),
                    Pin2 = Ports.Instance.GetStatePort(2).ToString(),
                    Pin3 = Ports.Instance.GetStatePort(3).ToString(),
                    Pin4 = Ports.Instance.GetStatePort(4).ToString(),
                    Pin5 = Ports.Instance.GetStatePort(5).ToString(),
                    Pin6 = Ports.Instance.GetStatePort(6).ToString(),
                    Pin7 = Ports.Instance.GetStatePort(7).ToString(),
                    Pin8 = Ports.Instance.GetStatePort(8).ToString(),
                    Pin9 = Ports.Instance.GetStatePort(9).ToString(),
                    Pin10 = Ports.Instance.GetStatePort(10).ToString(),
                    Pin11 = Ports.Instance.GetStatePort(11).ToString(),
                    Pin12 = Ports.Instance.GetStatePort(12).ToString()
                }
            };

            return JsonSerializer.SerializeObject(allPins.pins);
        }

        public class Pins
        {
            public string Pin0 { get; set; }

            public string Pin1 { get; set; }

            public string Pin2 { get; set; }

            public string Pin3 { get; set; }

            public string Pin4 { get; set; }

            public string Pin5 { get; set; }

            public string Pin6 { get; set; }

            public string Pin7 { get; set; }

            public string Pin8 { get; set; }

            public string Pin9 { get; set; }

            public string Pin10 { get; set; }

            public string Pin11 { get; set; }

            public string Pin12 { get; set; }

        }

        internal string MounthResponsePinJson(string port)
        {
            return "{\"Pin" + port + "\":\"" + Ports.Instance.GetStatePort(Convert.ToInt32(port)) + "\"}";
        }
    }




}
