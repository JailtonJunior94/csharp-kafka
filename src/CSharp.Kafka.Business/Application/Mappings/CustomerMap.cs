using System.Linq;
using System.Collections.Generic;
using CSharp.Kafka.Business.Domain.Dtos;
using CSharp.Kafka.Business.Domain.Entities;

namespace CSharp.Kafka.Business.Application.Mappings
{
    public static class CustomerMap
    {
        public static CustomerResponse ToResponse(this Customer customer)
        {
            return new CustomerResponse(customer.Id, customer.Name, customer.Email, customer.Active);
        }

        public static IEnumerable<CustomerResponse> ToListResponse(this IEnumerable<Customer> customer)
        {
            return customer.Select(j => new CustomerResponse(j.Id, j.Name, j.Email, j.Active));
        }
    }
}
