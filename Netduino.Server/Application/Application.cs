using Microsoft.SPOT;
using Netduino.Server.ValueObject;
using Netduino.Server.WebServer;

namespace Netduino.Server.Application
{
    public class Application
    {
        public string Path = "/api/pin";        
        private WebServer.WebServer _webServer;        

        public void Init()
        {
            initNetwork();            
            InitWebServer();

        }

        private void InitWebServer()
        {
            _webServer = new WebServer.WebServer(8080);
            _webServer.AddPathHandler(Path, HttpRequestMethod.Post, HttpRequestPostHandler);
            _webServer.AddPathHandler(Path, HttpRequestMethod.Get, HttpRequestGetHandler);
        }
      
        private void HttpRequestGetHandler(HttpContext context)
        {
            var request = new HttpRequestMessage(context);
            request.Process(Path);
        }

        private void HttpRequestPostHandler(HttpContext context)
        {
            var request = new HttpRequestMessage(context);
            request.Process(Path);
        }

        private void initNetwork()
        {
            Debug.EnableGCMessages(false);
            var networkInterface = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];

            Debug.Print("MEU IP ADDRESS: " + Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress);

        }

        public void Dispose()
        {
            _webServer.Stop();
        }
    }
}

