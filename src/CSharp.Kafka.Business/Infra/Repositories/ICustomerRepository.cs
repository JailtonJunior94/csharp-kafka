using System.Threading.Tasks;
using System.Collections.Generic;
using CSharp.Kafka.Business.Domain.Entities;

namespace CSharp.Kafka.Business.Infra.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAsync();
        Task<Customer> GetByIdAsync(long id);
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(long id);
    }
}
