﻿using System;

namespace CSharp.Kafka.Business.Domain.Entities
{
    public class Customer
    {
        public Customer(string name, string email)
        {
            Name = name;
            Email = email;
            CreatedAt = DateTime.Now;
            Active = true;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool Active { get; private set; }

        public Customer Update(string name, string email, bool active)
        {
            Name = name;
            Email = email;
            UpdatedAt = DateTime.Now;
            Active = active;

            return this;
        }
    }
}
