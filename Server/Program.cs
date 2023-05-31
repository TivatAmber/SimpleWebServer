namespace Server
{
    class Program
    {
        private const int ConcurrentCount = 20;
        private const string ServerUrl = "http://localhost:9000/";

        public static void Main(string[] args)
        {
            var server = new WebServer(ConcurrentCount);
            server.Bind(ServerUrl);
            server.Start();
            
            Console.WriteLine($"Web Server started at {ServerUrl}, Press any key to exit.");
            Console.ReadKey();
        }
    }
}