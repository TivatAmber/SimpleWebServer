namespace Server.Interfaces;

public interface IWebServerBuilder
{
    IWebServerBuilder Use(IMiddleware middleware);
    IWebServerBuilder UnhandleException(IExceptionHandler handler);
}