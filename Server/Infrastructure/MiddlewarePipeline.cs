using System.Net;
using Server.Interfaces;
using Server.Models;

namespace Server.Infrastructure;

public class MiddlewarePipeline
{
    public MiddlewarePipeline()
    {
        _middlewares = new List<IMiddleware>();
    }

    private List<IMiddleware> _middlewares;
    private IExceptionHandler _exceptionHandler;

    internal void Add(IMiddleware middleware)
    {
        _middlewares.Add(middleware);
    }

    internal void UnhandleException(IExceptionHandler handler)
    {
        _exceptionHandler = handler;
    }

    internal void Execute(HttpListenerContext context)
    {
        try
        {
            foreach (IMiddleware middleware in _middlewares)
            {
                MiddlewareResult result = middleware.Execute(context);
                if (result == MiddlewareResult.Processed) break;
                else if (result == MiddlewareResult.Continue) continue;
            }
        }
        catch (Exception ex)
        {
            if (_exceptionHandler != null) _exceptionHandler.HandleException(context, ex);
            else throw;
        }
    }
}