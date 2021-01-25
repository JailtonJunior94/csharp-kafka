using System.Threading.Tasks;
using CSharp.Kafka.Business.Domain.Dtos;

namespace CSharp.Kafka.Business.Application.Interfaces
{
    public interface ISlackService
    {
        Task SendMessageAsync(SlackRequest slack);
    }
}
