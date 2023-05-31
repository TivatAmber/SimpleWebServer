using Server.Infrastructure;
using Server.Interfaces;
using Server.Middlewares;
using Server.Models;

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
        Routing routes = new Routing();
        RegisterRoutes(routes);
        builder.Use(routes);
        
        builder.Use(new StaticFile());
        builder.Use(new Http404());
        
        builder.UnhandleException(new Http500());
    }

    static void RegisterRoutes(Routing routes)
    {
        routes.MapRoute(name:"StaticFile",
            url:"{controller}/{action}/{path}",
            defaults: new {controller = "StaticFile", action = "Index", path = UrlParameter.Optional}
        );
    }
}