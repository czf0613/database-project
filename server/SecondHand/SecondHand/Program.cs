using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SecondHand
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://*:12138")
                        .UseKestrel(option =>
                        {
                            option.Limits.MinRequestBodyDataRate = null;
                            option.Limits.MaxRequestBodySize = null;
                        })
                        .UseStartup<Startup>();
                });
    }
}