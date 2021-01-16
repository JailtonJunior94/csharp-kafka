using System.Threading.Tasks;
using CSharp.Kafka.Business.Domain.Messages;

namespace CSharp.Kafka.Business.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(KafkaMessage message);
    }
}
