using System.Net;
using System.Reflection;
using System.Text;
using Server.Infrastructure;
using Server.Interfaces;
using Server.Models;

namespace Server.Middlewares;

public class Routing : IMiddleware
{
    public Routing()
    {
        _entries = new List<RouteEntry>();
    }
    private List<RouteEntry> _entries;

    public Routing MapRoute(string name, string url, object defaults = null)
    {
        _entries.Add(new RouteEntry(name, url, defaults));
        return this;
    }

    public MiddlewareResult Execute(HttpListenerContext context)
    {
        foreach (RouteEntry entry in _entries)
        {
            RouteValueDictionary routeValues = entry.Match(context.Request);
            if (routeValues != null)
            {
                IController controller = CreateController(routeValues);
                MethodInfo method = GetActionMethod(controller, routeValues);
                byte[] result = GetAcionResult(controller, method, routeValues);
                context.Response.Status(200, "OK", result);
                return MiddlewareResult.Processed;
            }
        }

        return MiddlewareResult.Continue;
    }

    private IController CreateController(RouteValueDictionary routeValues)
    {
        string controllerName = (string)routeValues["controller"];
        string className = char.ToUpper(controllerName[0]) + controllerName.Substring(1) + "Controller";
        foreach (Type type in GetType().Assembly.GetExportedTypes())
        {
            if (type.Name == className && typeof(IController).IsAssignableFrom(type))
            {
                IController instance = (IController)Activator.CreateInstance(type);
                return instance;
            }
        }

        throw new ArgumentException($"Controller {className} Not Found");
    }

    private MethodInfo GetActionMethod(IController controller, RouteValueDictionary routeValues)
    {
        Type controllerType = controller.GetType();
        string actionName = (string)routeValues["action"];
        actionName = char.ToUpper(actionName[0]) + actionName.Substring(1);
        MethodInfo method = controller.GetType().GetMethod(actionName);
        if (method == null)
            Console.WriteLine($"Controller {controllerType} has no action method {actionName}");
        return method;
    }

    private byte[] GetAcionResult(IController controller, MethodInfo method, RouteValueDictionary routeValues)
    {
        ParameterInfo[] methodParams = method.GetParameters();
        object[] paramValues = new object[methodParams.Length];
        for (int i = 0; i < methodParams.Length; i++)
        {
            object routeValue = routeValues[methodParams[i].Name];
            object paramValue = Convert.ChangeType(routeValue, methodParams[i].ParameterType);
            paramValues[i] = paramValue;
        }

        byte[] result = (byte[])method.Invoke(controller, paramValues);
        return result;
    }
}