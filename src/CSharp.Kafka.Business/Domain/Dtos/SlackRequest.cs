using Newtonsoft.Json;

namespace CSharp.Kafka.Business.Domain.Dtos
{
    public class SlackRequest
    {
        public SlackRequest(string text)
        {
            Text = text;
        }

        [JsonProperty("text")]
        public string Text { get; private set; }
    }
}
