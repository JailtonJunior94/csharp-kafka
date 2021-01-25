using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using CSharp.Kafka.Business.Application.Interfaces;

namespace CSharp.Kafka.Business.Application.Services
{
    public class SlackService : ISlackService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SlackService> _logger;

        public SlackService(HttpClient httpClient, IConfiguration configuration, ILogger<SlackService> logger)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendMessageAsync(SlackRequest slack)
        {
            try
            {
                var body = new StringContent(JsonConvert.SerializeObject(slack), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_configuration["SlackBaseUrl"], body);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
            }
        }
    }
}
