using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Dtos;
using CSharp.Kafka.Business.Domain.Entities;
using CSharp.Kafka.Business.Infra.Repositories;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Business.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository repository, ILogger<CustomerService> logger)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ObjectResult> CreateUserAsync(CustomerRequest request)
        {
            try
            {
                var newCustomer = new Customer(request.Name, request.Email);

                var customer = await _repository.AddAsync(newCustomer);
                if (customer != null) return new ObjectResult(customer) { StatusCode = StatusCodes.Status201Created };

                return new ObjectResult(new { Errors = "Não foi possível cadastrar cliente!" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        public async Task<ObjectResult> DeleteUserAsync(long id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null) return new ObjectResult(new { Errors = "Cliente não encontrado!" }) { StatusCode = StatusCodes.Status404NotFound };

                var isRemoved = await _repository.DeleteAsync(id);
                if (isRemoved) return new ObjectResult(null) { StatusCode = StatusCodes.Status204NoContent };

                return new ObjectResult(new { Errors = "Não foi possível deletar o cliente!" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        public async Task<ObjectResult> GetUserById(long id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null) return new ObjectResult(new { Errors = "Cliente não encontrado!" }) { StatusCode = StatusCodes.Status404NotFound };

                return new ObjectResult(customer) { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        public async Task<ObjectResult> GetUsers()
        {
            try
            {
                var customers = await _repository.GetAsync();
                return new ObjectResult(customers) { StatusCode = StatusCodes.Status200OK };
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }

        public async Task<ObjectResult> UpdateUserAsync(long id, CustomerRequest request)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null) return new ObjectResult(new { Errors = "Cliente não encontrado!" }) { StatusCode = StatusCodes.Status404NotFound };

                var changed = await _repository.UpdateAsync(customer.Update(request.Name, request.Email));
                if (changed != null) return new ObjectResult(changed) { StatusCode = StatusCodes.Status200OK };

                return new ObjectResult(new { Errors = "Não foi possível alterar o cliente!" }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
