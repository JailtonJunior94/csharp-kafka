namespace CSharp.Kafka.Business.Domain.Messages
{
    public class CustomerMessage
    {
        public CustomerMessage(long id, string name, string email, long createdAt, long? updatedAt, bool active)
        {
            Id = id;
            Name = name;
            Email = email;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Active = active;
        }

        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public long CreatedAt { get; private set; }
        public long? UpdatedAt { get; private set; }
        public bool Active { get; private set; }
    }
}
