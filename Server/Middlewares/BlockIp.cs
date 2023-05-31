using System.Net;
using Server.Infrastructure;
using Server.Interfaces;
using Server.Models;

namespace Server.Middlewares;

public class BlockIp : IMiddleware
{
    public BlockIp(params string[] forbiddens)
    {
        _forbiddens = forbiddens;
    }

    private string[] _forbiddens;
    public MiddlewareResult Execute(HttpListenerContext context)
    {
        IPAddress clientIp = context.Request.RemoteEndPoint.Address;
        if (_forbiddens.Contains(clientIp.ToString()))
        {
            context.Response.Status(403, "Forbidden");
            return MiddlewareResult.Processed;
        }
        return MiddlewareResult.Continue;
    }
}