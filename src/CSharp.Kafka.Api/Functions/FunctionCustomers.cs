using System;
using NSwag.Annotations;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CSharp.Kafka.Business.Domain.Dtos;
using Microsoft.Azure.WebJobs.Extensions.Http;
using CSharp.Kafka.Business.Application.Interfaces;
using CSharp.Kafka.Business.Shared.ApplicationInsights;

namespace CSharp.Kafka.Api.Functions
{
    public class FunctionCustomers
    {
        private readonly ILogWithMetric _logger;
        private readonly ICustomerService _service;

        public FunctionCustomers(ILogWithMetric logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [SwaggerResponse(200, typeof(IEnumerable<CustomerResponse>))]
        [FunctionName("FunctionCustomers")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/customers")] HttpRequest req)
        {
            try
            {
                var result = await _service.GetCustomers();
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
