using System.Net;
using Server.Interfaces;

namespace Server.Infrastructure;

public class WebServer : IWebServerBuilder
{
    private readonly Semaphore _sem;
    private readonly HttpListener _listener;
    private readonly MiddlewarePipeline _pipeline;

    public WebServer(int concurrentCount)
    {
        _sem = new Semaphore(concurrentCount, concurrentCount);
        _listener = new HttpListener();
        _pipeline = new MiddlewarePipeline();
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
                    _pipeline.Execute(context);
                }
            }
        );
    }

    public IWebServerBuilder Use(IMiddleware middleware)
    {
        _pipeline.Add(middleware);
        return this;
    }

    public IWebServerBuilder UnhandleException(IExceptionHandler handler)
    {
        _pipeline.UnhandleException(handler);
        return this;
    }
}