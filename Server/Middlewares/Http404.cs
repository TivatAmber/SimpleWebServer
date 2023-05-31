using System.Net;
using Server.Infrastructure;
using Server.Interfaces;
using Server.Models;

namespace Server.Middlewares;

public class Http404 : IMiddleware
{
    public MiddlewareResult Execute(HttpListenerContext context)
    {
        context.Response.Status(404, "File Not Found");
        return MiddlewareResult.Processed;
    }   
}