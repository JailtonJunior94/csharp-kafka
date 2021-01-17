using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Messages;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Business.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public async Task SendNotificationAsync(KafkaMessage message)
        {
            if (message == null)
            {
                _logger.LogInformation($"[DELETADO] - ");
            }

            if (message?.Payload.After != null && message.Payload?.Before == null)
            {
                _logger.LogInformation("[INSERIDO] - ");
            }
            else
            {
                _logger.LogInformation("[ALTERADO] - ");

            }
        }
    }
}
