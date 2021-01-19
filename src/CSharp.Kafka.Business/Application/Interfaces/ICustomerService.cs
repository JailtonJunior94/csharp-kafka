using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharp.Kafka.Business.Domain.Dtos;

namespace CSharp.Kafka.Business.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ObjectResult> GetUsers();
        Task<ObjectResult> GetUserById(long id);
        Task<ObjectResult> CreateUserAsync(CustomerRequest request);
        Task<ObjectResult> UpdateUserAsync(long id, CustomerRequest request);
        Task<ObjectResult> DeleteUserAsync(long id);
    }
}
