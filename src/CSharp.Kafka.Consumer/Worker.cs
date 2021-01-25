using System;
using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly INotificationService _service;
        private readonly IConfiguration _configuration;
        private readonly IConsumer<string, string> _consumer;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Worker(ILogger<Worker> logger, INotificationService service, IConfiguration configuration)
        {
            _logger = logger;
            _service = service;
            _configuration = configuration;
            _cancellationTokenSource = new CancellationTokenSource();

            var config = new ConsumerConfig
            {
                GroupId = _configuration["GroupId"],
                BootstrapServers = _configuration["BootstrapServer"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _consumer.Subscribe(_configuration["TopicName"]);
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

                    await _service.SendNotificationAsync(consume);

                    _consumer.Commit();
                }
                catch (ConsumeException exception)
                {
                    _logger.LogError($"{exception.Error.Reason}");
                }
                catch (OperationCanceledException exception)
                {
                    _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                }
                catch (Exception exception)
                {
                    _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                }
            }
        }
    }
}
