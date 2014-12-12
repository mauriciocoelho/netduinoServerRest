using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using Json.NETMF;
using Microsoft.SPOT;

namespace Netduino.Server.WebServer
{
    public class HttpResponseMessage
    {
        public HttpContext Context { get; private set; }

        public HttpContext HttpContext
        {
            get
            {
                throw new NotImplementedException();
            }
            set { if (value == null) throw new ArgumentNullException("value"); }
        }

        public HttpResponseMessage(HttpContext context)
        {
            Context = context;
        }

        public void Process(string response, string contentType)
        {
            if (Context.ClientSocket != null)
            {
                string header = "HTTP/1.0 200 OK\r\n"
                    + "Content-Type: " + contentType + "; charset=utf-8\r\n"
                    + "Content-Length: " + response.Length.ToString() + "\r\n"
                    + "Connection: close\r\n"
                    + "\r\n";
                TrySend(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);
                TrySend(Encoding.UTF8.GetBytes(response), response.Length, SocketFlags.None);
            }
        }

        private void TrySend(byte[] buffer, int size, SocketFlags socketFlags)
        {
            Context.ClientSocket.Send(buffer, size, socketFlags);
        }

        public void SendResponseJson(Hashtable hashtable)
        {            
            Process(JsonSerializer.SerializeObject(hashtable), "application/json");
        }

        public void SendNotFoundResponse()
        {
            const string header = "HTTP/1.1 404 Not Found\r\nContent-Length: 0\r\nConnection: close\r\n\r\n";
            if (Context.ClientSocket != null)
                TrySend(Encoding.UTF8.GetBytes(header), header.Length, SocketFlags.None);

            Debug.Print("Sent: 404 Not Found response.");
        }
    }
}
