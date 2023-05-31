using System.Net;
using Server.Interfaces;
using Server.Models;

namespace Server.Middlewares;

public class HttpLog : IMiddleware
{
    public MiddlewareResult Execute(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        string path = request.Url.LocalPath;
        IPAddress clientIp = request.RemoteEndPoint.Address;
        string method = request.HttpMethod;
        
        Console.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1} {2} {3}", DateTime.Now, clientIp, method, path);

        return MiddlewareResult.Continue;
    }
}