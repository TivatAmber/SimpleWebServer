using System.Net;
using Server.Infrastructure;
using Server.Interfaces;

namespace Server.Middlewares;

public class Http500 : IExceptionHandler
{
    public void HandleException(HttpListenerContext context, Exception exp)
    {
        Console.WriteLine(exp.Message);
        Console.WriteLine(exp.StackTrace);
        context.Response.Status(500, "Internet Server Error");
    }
}