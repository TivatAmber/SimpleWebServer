using System.Text;
using Server.Infrastructure;

namespace Server.Controllers;

public class StaticFileController : Controller
{
    public byte[] GetFile(string path)
    {
        string filePath = Path.Combine("files", path);
        Console.WriteLine(filePath);
        if (File.Exists(filePath))
        {
            byte[] data = File.ReadAllBytes(filePath);
            return data;
        }

        return Encoding.UTF8.GetBytes("Not Found");
    }

    public byte[] Upload()
    {
        return Encoding.UTF8.GetBytes("Successfully Upload");
    }
    public byte[] Index()
    {
        // return "Index Pages";
        string rootPath = Directory.GetCurrentDirectory();
        string path = Path.Combine(rootPath, "files");
        Console.WriteLine(path);
        return Encoding.UTF8.GetBytes(GetDir(path));
    }

    private string GetDir(string path)
    {
        DirectoryInfo nowDir = new DirectoryInfo(path);
        string[] files = nowDir.GetFiles().Select(x => GetShortPath(x.FullName)).ToArray();
        string[] dirs = nowDir.GetDirectories().Select(x => GetDir(x.FullName)).ToArray();
        return string.Join('\n', files) + "\n" + string.Join('\n', dirs);
    }

    private string GetShortPath(string path)
    {
        int idx = path.IndexOf("files\\") + "files\\".Length;
        return path.Substring(idx, path.Length - idx);
    }
}