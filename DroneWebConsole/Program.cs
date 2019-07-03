using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DroneWebConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            /*var host = new WebHostBuilder()
                                .UseKestrel()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseUrls("http://192.168.138.1:5567")
                                .UseIISIntegration()
                                .UseStartup<Startup>()
                                .Build();
            host.Run();*/
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
