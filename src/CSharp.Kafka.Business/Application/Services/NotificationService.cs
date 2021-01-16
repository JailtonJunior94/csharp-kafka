using System;
using System.Threading.Tasks;
using CSharp.Kafka.Business.Domain.Messages;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Business.Application.Services
{
    public class NotificationService : INotificationService
    {
        public NotificationService()
        {

        }

        public Task SendNotificationAsync(KafkaMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
