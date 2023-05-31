using System.Net;
using System.Text;

namespace Server
{
    public class WebServer
    {
        private readonly Semaphore _sem;
        private readonly HttpListener _listener;

        public WebServer(int concurrentCount)
        {
            _sem = new Semaphore(concurrentCount, concurrentCount);
            _listener = new HttpListener();
        }

        public void Bind(string url)
        {
            _listener.Prefixes.Add(url);
        }

        public void Start()
        {
            _listener.Start();

            Task.Run(async () =>
                {
                    while (true)
                    {
                        _sem.WaitOne();
                        HttpListenerContext context = await _listener.GetContextAsync();
                        _sem.Release();
                        HandleContext(context);
                    }
                }
            );
        }

        private void HandleContext(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            string urlPath = request.Url.LocalPath.TrimStart('/');
            Console.WriteLine($"url path = {urlPath}");

            try
            {
                string filePath = Path.Combine("files", urlPath);
                byte[] data = File.ReadAllBytes(filePath);
                Console.WriteLine($"Try to get {filePath}");
                response.ContentType = "text/html";
                response.ContentLength64 = data.Length;
                response.ContentEncoding = Encoding.UTF8;
                response.StatusCode = 200;
                response.OutputStream.Write(data, 0, data.Length);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}