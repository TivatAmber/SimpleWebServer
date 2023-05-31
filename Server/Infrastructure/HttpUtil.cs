using System.Net;
using System.Text;

namespace Server.Infrastructure;

public static class HttpUtil
{
    public static HttpListenerResponse Status(this HttpListenerResponse response, int statusCode, string description)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(description);

        response.StatusCode = statusCode;
        response.StatusDescription = description;
        response.ContentLength64 = messageBytes.Length;
        response.OutputStream.Write(messageBytes, 0, messageBytes.Length);
        response.OutputStream.Close();
        
        return response;
    }
}