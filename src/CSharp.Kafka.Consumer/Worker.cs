using System;
using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Messages;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly INotificationService _service;

        public Worker(ILogger<Worker> logger,
                      INotificationService service)
        {
            _logger = logger;
            _service = service;
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
                        _logger.LogInformation("Start consumer message");

                        consumer.Subscribe("dbserver1.dbo.Customers");
                        var consume = consumer.Consume();

                        if (consume.Message?.Value == null) return;

                        _logger.LogInformation($"Send message to notification: {consume.Message.Value}");

                        var message = JsonConvert.DeserializeObject<KafkaMessage>(consume.Message.Value);
                        await _service.SendNotificationAsync(message);
                    }

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                }
            }
        }
    }
}
