namespace CSharp.Kafka.Business.Domain.Dtos
{
    public class CustomerResponse
    {
        public CustomerResponse(long id, string name, string email, bool active)
        {
            Id = id;
            Name = name;
            Email = email;
            Active = active;
        }

        public long Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool Active { get; private set; }
    }
}
