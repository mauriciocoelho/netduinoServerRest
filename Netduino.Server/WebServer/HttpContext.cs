using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Server.ValueObject;

namespace Netduino.Server.WebServer
{

    public class HttpContext : IDisposable
    {
        public string Url { get; private set; }

        public HttpRequestMethod Method { get; private set; }

        public Socket ClientSocket { get; private set; }

        public Hashtable Header { get; private set; }

        private const int BufferSize = 4096; //8192; //1536;
        
        private IPAddress _initialIpAdress = IPAddress.Any;

        internal HttpContext(Socket clientSocket, char[] headerData)
        {
            Header = new Hashtable();
            ClientSocket = clientSocket;
            InitialIpAdress = ClientIpAddress;
            ProcessRequest(headerData);
        }

        public IPAddress ClientIpAddress
        {
            get
            {
                var ip = ClientSocket.RemoteEndPoint as IPEndPoint;
                if (ip != null) return ip.Address;
                return null;
            }
        }

        public IPAddress InitialIpAdress
        {
            get { return _initialIpAdress; }
            set { _initialIpAdress = value; }
        }

        public string ReceiveData()
        {
            var contentLength = Header["Content-Length"].ToString() ?? String.Empty;            
            var length = int.Parse(contentLength);
            
            var buffer = new byte[BufferSize];
            var receiveBuffer = new byte[0];

            while (length > 0)
            {
                int availableBytes = WebServer.WaitForSocketAvailable(ClientSocket);

                int size = (availableBytes > BufferSize ? BufferSize : availableBytes);
                size = (size > length ? length : size);

                var received = ClientSocket.Receive(buffer, size, SocketFlags.None);
                if (received > 0)
                {
                    receiveBuffer = Utility.CombineArrays(receiveBuffer,
                        Utility.ExtractRangeFromArray(buffer, 0, received));
                    length -= received;
                }
            }
          

            var result = Encoding.UTF8.GetChars(receiveBuffer);
            return new string(result);
        }  

        private void ProcessRequest(char[] headerData)
        {
            var content = new string(headerData);
            Debug.Print(content);
            var headerLines = content.Split('\n');

            string firstLine = headerLines[0];
            string[] words = firstLine.Split(' ');
            var method = words[0];

            Url = UnescapeString(words[1]);

            switch (method)
            {
                case "GET":
                    Method = HttpRequestMethod.Get;
                    break;
                case "POST":
                    Method = HttpRequestMethod.Post;
                    break;
                default:
                    Method = HttpRequestMethod.Unknown;
                    break;
            }            
            foreach (var line in headerLines)
            {
                if (line.IndexOf(':') < 0)
                    continue;

                var lineParts = line.Split(':');

                var key = lineParts[0].Trim(' ', '\t');
                var value = lineParts[1].Trim(' ', '\t', '\r');
                if (!Header.Contains(key))
                    Header.Add(key, value);
            }
        }

        string UnescapeString(string s)
        {
            int idx = s.IndexOf('%'), beg = 0;
            if (idx < 0) return s;
            var sb = new StringBuilder();
            var bytes = new ArrayList();
            while (idx >= 0 && idx < s.Length - 2)
            {
                if (idx > beg) sb.Append(s.Substring(beg, idx - beg));
                do
                {
                    var hex = s.Substring(idx + 1, 2);
                    byte val;
                    if (TryParseHex(hex, out val))
                    {
                        bytes.Add(val);
                        idx += 3;
                    }
                    else
                    {
                        idx++;
                    }
                } while (idx < s.Length - 2 && s[idx] == '%');
                var bs = (byte[])bytes.ToArray(typeof(byte));
                sb.Append(Encoding.UTF8.GetChars(bs));
                bytes.Clear();
                beg = idx;
                idx = s.IndexOf('%', idx);
            }
            if (beg < s.Length) sb.Append(s.Substring(beg));
            return sb.ToString();
        }       

        bool TryParseHex(string s, out byte b)
        {
            int res = 0;
            for (int i = 0; i < s.Length; i++)
            {
                res *= 16;
                char c = s[i];
                int val;
                if (c >= '0' && c <= '9')
                {
                    val = c - '0';
                }
                else if (c >= 'a' && c <= 'f')
                {
                    val = c - 'a' + 10;
                }
                else if (c >= 'A' && c <= 'F')
                {
                    val = c - 'A' + 10;
                }
                else
                {
                    b = 0;
                    return false;
                }
                res += val;
            }
            b = (byte)res;
            return true;
        }

        public void Dispose()
        {
            if (ClientSocket != null)
            {
                ClientSocket.Close();
                ClientSocket = null;
            }
        }

    }   

}
