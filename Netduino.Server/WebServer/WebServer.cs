using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Server.ValueObject;

namespace Netduino.Server.WebServer
{
    public delegate void MessageDelegate(HttpContext context);

    public struct HttpMessageHandler
    {
        public string Path { get; set; }
        public HttpRequestMethod Method { get; set; }
        public MessageDelegate Handler { get; set; }
    }

    public class WebServer : IDisposable
    {        
        private const int MaxRequestSize = 256;
        public readonly int PortNumber = 8080;
        private readonly Socket _listeningSocket;
        private readonly ArrayList _pathHandlers = new ArrayList();
        private readonly Thread _workerThread;

        public WebServer() : this(8080) { }
        public WebServer(int portNumber)
        {
            PortNumber = portNumber;
            _listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listeningSocket.Bind(new IPEndPoint(IPAddress.Any, PortNumber));
            _listeningSocket.Listen(10);

            _workerThread = new Thread(StartListening);
            _workerThread.Start();
        }

        public bool Stop()
        {
            if (_workerThread == null)
                return false;

            if (_listeningSocket != null)
                _listeningSocket.Close();

            _workerThread.Abort();
            return true;
        }

        public void AddPathHandler(string path, HttpRequestMethod method, MessageDelegate handler)
        {
            var pathHandler = new HttpMessageHandler { Path = path.ToLower(), Method = method, Handler = handler };
            _pathHandlers.Add(pathHandler);
        }

        private void OnRequestReceived(HttpContext request)
        {
            foreach (HttpMessageHandler pathHandler in _pathHandlers)
            {
                if (pathHandler.Method != request.Method
                    && pathHandler.Method != HttpRequestMethod.Any)
                    continue;

                var path = pathHandler.Path;
                var url = request.Url.ToLower();
                var match = path == url.Substring(0, path.Length);

                if (!match)
                {
                    var pathLength = path.Length;
                    if (path[pathLength - 1] != '*')
                        continue;

                    path = path.Substring(0, pathLength - 1);
                    match = url.IndexOf(path) == 0;
                    if (!match)
                        continue;
                }
                pathHandler.Handler(request);
                break;
            }
        }

        public void StartListening()
        {
            while (true)
            {
                try
                {
                    using (Socket clientSocket = _listeningSocket.Accept())
                    {
                        clientSocket.ReceiveTimeout = 200;
                        clientSocket.SendTimeout = 1000;

                        ResponseRequest(clientSocket);
                    }
                }
                catch (SocketException e)
                {
                    Debug.Print(e.Message + " Error Code: " + e.ErrorCode.ToString());
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
                Thread.Sleep(10);
            }
        }

        private void ResponseRequest(Socket clientSocket)
        {
            var clientIp = clientSocket.RemoteEndPoint as IPEndPoint;
            if (clientIp != null) Debug.Print("Received request from " + clientIp);

            bool complete = false;
            var headerBuffer = new byte[0];

            while (!complete)
            {
                var availableBytes = WaitForSocketAvailable(clientSocket);
                if (availableBytes < 0)
                {
                    Debug.Print("*** Socket unavailable.");
                    return;
                }
                int bytesReceived = (availableBytes > MaxRequestSize ? MaxRequestSize : availableBytes);
                if (bytesReceived > 0)
                {
                    var buffer = GetRequestHeader(clientSocket, bytesReceived, out complete);
                    headerBuffer = Utility.CombineArrays(headerBuffer, buffer);
                }
            }

            var headerData = Encoding.UTF8.GetChars(headerBuffer);
            using (var request = new HttpContext(clientSocket, headerData))
            {                
                OnRequestReceived(request);
            }
        }

        public static int WaitForSocketAvailable(Socket clientSocket)
        {
            return WaitForSocketAvailable(clientSocket, 500);
        }

        public static int WaitForSocketAvailable(Socket clientSocket, int delay)
        {
            var dataReady = clientSocket.Poll(-1, SelectMode.SelectRead);
            var available = clientSocket.Available;
            if (dataReady && available > 0)
                return available;
            return -1;
        }

        private enum HeaderState
        {
            None,
            FirstCr,
            FirstNl,
            SecondCr,
            SecondNl
        }

        private byte[] GetRequestHeader(Socket clientSocket, int bytesReceived, out bool complete)
        {
            complete = false;
            var state = HeaderState.None;
            var received = 0;

            var buffer = new byte[bytesReceived];
            var charBuffer = new byte[1];
            while (clientSocket.Receive(charBuffer, 1, SocketFlags.None) == 1)
            {
                var ch = charBuffer[0];
                buffer[received++] = ch;

                switch (state)
                {
                    case HeaderState.None:
                        if (ch == 13) state = HeaderState.FirstCr;
                        break;
                    case HeaderState.FirstCr:
                        state = (ch == 10) ? HeaderState.FirstNl : HeaderState.None;
                        break;
                    case HeaderState.FirstNl:
                        state = (ch == 13) ? HeaderState.SecondCr : HeaderState.None;
                        break;
                    case HeaderState.SecondCr:
                        state = (ch == 10) ? HeaderState.SecondNl : HeaderState.None;
                        break;
                    case HeaderState.SecondNl:
                        break;
                }

                if (state == HeaderState.SecondNl)
                {
                    complete = true;
                    break;
                }

                if (received >= bytesReceived)
                    break;
            }

            byte[] result = Utility.ExtractRangeFromArray(buffer, 0, received);
            return result;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_listeningSocket != null) _listeningSocket.Close();

        }

        #endregion

        public HttpContext HttpContext
        {
            get
            {
                throw new NotImplementedException();
            }
            set { if (value == null) throw new ArgumentNullException("value"); }
        }

        public HttpMessageHandler HttpMessageHandler
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
