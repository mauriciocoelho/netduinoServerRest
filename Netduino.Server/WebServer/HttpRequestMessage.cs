using System;
using System.Net;
using Netduino.Server.Infrastructure;
using Netduino.Server.ValueObject;
using System.Collections;
using Json.NETMF;

namespace Netduino.Server.WebServer
{
    public class HttpRequestMessage
    {
        public HttpContext Context { get; private set; }

        public HttpContext HttpContext
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public HttpRequestMessage(HttpContext context)
        {
            Context = context;
        }

        public HttpStatusCode Process(string path)
        {
            switch (Context.Method)
            {
                case HttpRequestMethod.Get:
                    ProcessGetMethod(path);
                    break;

                case HttpRequestMethod.Post:
                    ProcessPostMethod();
                    break;
            }

            return HttpStatusCode.OK;
        }

        private void ProcessPostMethod()
        {
            var data = Context.ReceiveData();
            if (data == null) return;
            
            PinReceiveBody pinReceiveBody = MounthPinReceiveBody((Hashtable)JsonSerializer.DeserializeString(data));                            
            GenerateResponse(pinReceiveBody);
        }

        private PinReceiveBody MounthPinReceiveBody(Hashtable hashtable)
        {
            var pinReceiveBody = new PinReceiveBody();
            foreach (DictionaryEntry entry in hashtable)
            {
                if (entry.Key.Equals("Status")) pinReceiveBody.Status = !entry.Value.ToString().Equals("False");
                else if (entry.Key.Equals("Pin")) pinReceiveBody.Pin = Int32.Parse(entry.Value.ToString());
            }
            return pinReceiveBody;
        }

        private void GenerateResponse(PinReceiveBody pinReceiveBody)
        {
            Hashtable sucess;
            if (pinReceiveBody.Pin >= 0 && pinReceiveBody.Pin <= 13)
            {
                Ports.Instance.SetPort(pinReceiveBody);
                sucess = new Hashtable {{"Sucess", true}};
            }
            else
                sucess = new Hashtable {{"Sucess", false}};

            var response = new HttpResponseMessage(Context);
            response.SendResponseJson(sucess);
        }

        private void ProcessGetMethod(string path)
        {
            string json = string.Empty;
            if (Context.Url.Length - 1 > path.Length)
                json = AllPinsResponseBody.Instance.MounthResponsePinJson(Context.Url.Substring(path.Length + 1));
            else if (string.Equals(Context.Url.Length, path.Length))
                json = AllPinsResponseBody.Instance.MounthResponsePinsJson();

            var response = new HttpResponseMessage(Context);
            response.Process(json, "application/js");
        }
    }
}
