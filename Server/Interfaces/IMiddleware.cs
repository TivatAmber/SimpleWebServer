using System.Net;
using Server.Models;
namespace Server.Interfaces;

public interface IMiddleware
{
    MiddlewareResult Execute(HttpListenerContext context);
}