using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CSharp.Kafka.Business.Application.Services;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<INotificationService, NotificationService>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
