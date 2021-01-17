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
        private readonly IConsumer<string, string> _consumer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Worker(ILogger<Worker> logger, INotificationService service)
        {
            _logger = logger;
            _service = service;
            _cancellationTokenSource = new CancellationTokenSource();

            var config = new ConsumerConfig
            {
                GroupId = $"dbserver.dbo.Customers.group.id",
                BootstrapServers = "192.168.0.109:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe("dbserver.dbo.Customers");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _cancellationTokenSource.Cancel();
            };

            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                try
                {
                    var consume = _consumer.Consume(_cancellationTokenSource.Token);
                    _logger.LogInformation($"Consumed message '{consume.Message?.Value}' at: '{consume.TopicPartitionOffset}'.");

                    KafkaMessage message = null;
                    if (consume.Message.Value != null)
                        message = JsonConvert.DeserializeObject<KafkaMessage>(consume?.Message?.Value);

                    await _service.SendNotificationAsync(message);
                    _consumer.Commit();
                }
                catch (ConsumeException exception)
                {
                    _logger.LogError($"Error occured: {exception.Error.Reason}");
                }
                catch (OperationCanceledException exception)
                {
                    _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                    _consumer.Close();
                }
                catch (Exception exception)
                {
                    _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                }
            }
        }
    }
}
