using Confluent.Kafka;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Dtos;
using CSharp.Kafka.Business.Domain.Messages;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Business.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ISlackService _slackService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger, ISlackService slackService)
        {
            _logger = logger;
            _slackService = slackService;
        }

        public async Task SendNotificationAsync(ConsumeResult<string, string> consume)
        {
            KafkaMessage message = null;

            if (consume.Message.Value != null)
                message = JsonConvert.DeserializeObject<KafkaMessage>(consume?.Message?.Value);

            string text;
            if (message == null)
            {
                _logger.LogInformation($"[DELETE] - ");
                text = $"[DELETADO] - O Cliente foi deletado";

                await _slackService.SendMessageAsync(new SlackRequest(text));
            }

            if (message?.Payload.After != null && message.Payload?.Before == null)
            {
                _logger.LogInformation("[INSERT] - ");

                text = $@"[INSERIDO] Cliente:       
                            Id: {message?.Payload?.After?.Id}
                            Nome: {message?.Payload?.After?.Name}
                            E-mail: {message?.Payload?.After?.Email}";

                await _slackService.SendMessageAsync(new SlackRequest(text));
            }
            else
            {
                _logger.LogInformation("[UPDATE] - ");

                text = $@"[ALTERADO] Cliente:
                          ANTES:
                           Id: {message?.Payload?.Before?.Id}
                           Nome: {message?.Payload?.Before?.Name}
                           E-mail: {message?.Payload?.Before?.Email}
                          DEPOIS:
                           Id: {message?.Payload?.After?.Id}
                           Nome: {message?.Payload?.After?.Name}
                           E-mail: {message?.Payload?.After?.Email}";

                await _slackService.SendMessageAsync(new SlackRequest(text));
            }
        }
    }
}
