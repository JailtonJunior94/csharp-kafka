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
    public class FunctionDeleteCustomer
    {
        private readonly ILogWithMetric _logger;
        private readonly ICustomerService _service;

        public FunctionDeleteCustomer(ILogWithMetric logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [SwaggerResponse(200, typeof(CustomerResponse))]
        [FunctionName("FunctionDeleteCustomer")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/customers/{id:long}")] HttpRequest req, long id)
        {
            try
            {
                var result = await _service.DeleteCustomerAsync(id);
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
