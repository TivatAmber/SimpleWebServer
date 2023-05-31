using System.Net;

namespace Server.Interfaces;

public interface IExceptionHandler
{
    void HandleException(HttpListenerContext context, Exception exp);
}