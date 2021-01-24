using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharp.Kafka.Business.Domain.Dtos;

namespace CSharp.Kafka.Business.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ObjectResult> GetCustomers();
        Task<ObjectResult> GetCustomerById(long id);
        Task<ObjectResult> CreateCustomerAsync(CustomerRequest request);
        Task<ObjectResult> UpdateCustomerAsync(long id, CustomerRequest request);
        Task<ObjectResult> DeleteCustomerAsync(long id);
    }
}
