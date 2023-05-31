using System.Net;
using System.Text;
using Server.Interfaces;
using Server.Models;

namespace Server.Middlewares;

public class StaticFile : IMiddleware
{
    public MiddlewareResult Execute(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;
        string urlPath = request.Url.LocalPath.TrimStart('/');
        string filePath = Path.Combine("files", urlPath);
        if (File.Exists(filePath))
        {
            byte[] data = File.ReadAllBytes(filePath);
            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = data.Length;
            response.StatusCode = 200;
            response.OutputStream.Write(data, 0, data.Length);
            response.OutputStream.Close();
        }
        
        return MiddlewareResult.Processed;
    }
}