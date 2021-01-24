using Confluent.Kafka;
using System.Threading.Tasks;

namespace CSharp.Kafka.Business.Application.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(ConsumeResult<string, string> consume);
    }
}
