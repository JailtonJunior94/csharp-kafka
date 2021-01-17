using System.Collections.Generic;

namespace CSharp.Kafka.Business.Domain.Messages
{
    public class KafkaMessage
    {
        public SchemaPrimary Schema { get; set; }
        public Payload Payload { get; set; }
    }

    public class SchemaPrimary
    {
        public string Type { get; set; }
        public ICollection<FieldsPrimary> Fields { get; set; }
        public bool Optional { get; set; }
        public string Name { get; set; }
    }

    public class FieldsPrimary
    {
        public string Type { get; set; }
        public ICollection<FieldChield> Fields { get; set; }
        public bool Optional { get; set; }
        public string Field { get; set; }
        public string Name { get; set; }
    }

    public class FieldChield
    {
        public string Type { get; set; }
        public bool Optional { get; set; }
        public string Field { get; set; }
    }

    public class Payload
    {
        public CustomerMessage Before { get; set; }
        public CustomerMessage After { get; set; }
    }

    public class KeyIndentification
    {
        public Identification Payload { get; set; }
    }

    public class Identification
    {
        public int Id { get; set; }
    }
}