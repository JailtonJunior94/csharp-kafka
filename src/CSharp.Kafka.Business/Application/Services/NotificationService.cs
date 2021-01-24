using Confluent.Kafka;
using Newtonsoft.Json;
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

        public async Task SendNotificationAsync(ConsumeResult<string, string> consume)
        {
            KafkaMessage message = null;
            if (consume.Message.Value != null)
                message = JsonConvert.DeserializeObject<KafkaMessage>(consume?.Message?.Value);

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
