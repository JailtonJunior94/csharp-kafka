using System;
using NSwag.Annotations;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CSharp.Kafka.Business.Domain.Dtos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using CSharp.Kafka.Business.Application.Interfaces;
using CSharp.Kafka.Business.Shared.ApplicationInsights;

namespace CSharp.Kafka.Api.Functions
{
    public class FunctionCustomerById
    {
        private readonly ILogWithMetric _logger;
        private readonly ICustomerService _service;

        public FunctionCustomerById(ILogWithMetric logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [SwaggerResponse(200, typeof(CustomerResponse))]
        [FunctionName("FunctionCustomerById")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/customers/{id:long}")] HttpRequest req, long id)
        {
            try
            {
                var result = await _service.GetCustomerById(id);
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
