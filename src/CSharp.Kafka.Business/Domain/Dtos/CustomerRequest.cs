namespace CSharp.Kafka.Business.Domain.Dtos
{
    public class CustomerRequest
    {
        public CustomerRequest(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
    }
}
