using System;
using CSharp.Kafka.Business.Shared.Extensions;

namespace CSharp.Kafka.Business.Domain.Entities
{
    public class Customer
    {
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
            CreatedAt = DateTime.Now.UtcBrazil();
            Active = true;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool Active { get; private set; }

        public Customer SetId(long id)
        {
            Id = id;
            return this;
        }

        public Customer Update(string name, string email)
        {
            Name = name;
            Email = email;
            UpdatedAt = DateTime.Now.UtcBrazil();

            return this;
        }
    }
}
