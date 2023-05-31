using Server.Infrastructure;
using Server.Interfaces;
using Server.Middlewares;

namespace Server;

class Program
{
    private const int ConcurrentCount = 20;
    private const string ServerUrl = "http://localhost:9000/";

    public static void Main(string[] args)
    {
        var server = new WebServer(ConcurrentCount);
        
        RegisterMiddlewares(server);
        
        server.Bind(ServerUrl);
        server.Start();

        Console.WriteLine($"Web Server started at {ServerUrl}, Press any key to exit.");
        Console.ReadKey();
    }

    static void RegisterMiddlewares(IWebServerBuilder builder)
    {
        builder.Use(new HttpLog());
        // builder.Use(new Http404());
        builder.Use(new StaticFile());
        
        builder.UnhandleException(new Http500());
    }
}