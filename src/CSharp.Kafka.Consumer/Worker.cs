using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CSharp.Kafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = $"dbserver1.dbo.Customers.{Guid.NewGuid():N}group.id",
                BootstrapServers = "192.168.0.109:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    using (var consumer = new ConsumerBuilder<string, string>(config).Build())
                    {
                        consumer.Subscribe("dbserver1.dbo.Customers");
                        var consume = consumer.Consume();

                        _logger.LogInformation(consume.Message.Value);

                        var message = consume.Message;
                    }
                        
                    

                 


                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception exception)
                {
                    
                }
            }
        }
    }
}
