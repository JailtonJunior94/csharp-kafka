using System;
using Dapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Infra.Queries;
using CSharp.Kafka.Business.Domain.Entities;

namespace CSharp.Kafka.Business.Infra.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(ILogger<CustomerRepository> logger)
        {
            _logger = logger;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            _logger.LogInformation("[INSERT] - Iniciando a inserção na tabela dbo.Customers");
            try
            {
                using var connection = GetConnection();
                var id = await connection.QuerySingleAsync<long>(CustomerQuery.Add, new
                {
                    name = customer.Name,
                    email = customer.Email,
                    createdAt = customer.CreatedAt,
                    updatedAt = customer.UpdatedAt,
                    active = customer.Active
                });

                return customer.SetId(id);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            _logger.LogInformation("[DELETE] - Iniciando a removação na tabela dbo.Customers");
            try
            {
                using var connection = GetConnection();
                var row = await connection.ExecuteAsync(CustomerQuery.Delete, new { id });

                if (row > 0) return true;
                return false;
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<Customer>> GetAsync()
        {
            _logger.LogInformation("[SELECT] - Iniciando a consulta na tabela dbo.Customers");
            try
            {
                using var connection = GetConnection();
                var customers = await connection.QueryAsync<Customer>(CustomerQuery.Get);

                return customers;
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return null;
            }
        }

        public async Task<Customer> GetByIdAsync(long id)
        {
            _logger.LogInformation("[SELECT] - Iniciando a consulta na tabela dbo.Customers");
            try
            {
                using var connection = GetConnection();
                var customer = await connection.QueryFirstAsync<Customer>(CustomerQuery.GetById, new { id });

                return customer;
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return null;
            }
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _logger.LogInformation("[UPDATE] - Iniciando update na tabela dbo.Customers");
            try
            {
                using var connection = GetConnection();
                var row = await connection.ExecuteAsync(CustomerQuery.Update, new
                {
                    id = customer.Id,
                    name = customer.Name,
                    email = customer.Email
                });

                if (row > 0) return await GetByIdAsync(customer.Id);
                return null;
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return null;
            }
        }
    }
}
