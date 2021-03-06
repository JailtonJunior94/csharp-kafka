﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NSwag.Annotations;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CSharp.Kafka.Business.Domain.Dtos;
using NSwag.Annotations.AzureFunctionsV2;
using Microsoft.Azure.WebJobs.Extensions.Http;
using CSharp.Kafka.Business.Application.Interfaces;
using CSharp.Kafka.Business.Application.Validations;
using CSharp.Kafka.Business.Shared.ApplicationInsights;

namespace CSharp.Kafka.Api.Functions
{
    public class FunctionUpdateCustomer
    {
        private readonly ILogWithMetric _logger;
        private readonly ICustomerService _service;

        public FunctionUpdateCustomer(ILogWithMetric logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [SwaggerRequestBodyType(typeof(CustomerRequest))]
        [SwaggerResponse(200, typeof(CustomerResponse))]
        [FunctionName("FunctionUpdateCustomer")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/customers/{id:long}")] HttpRequest req, long id)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<CustomerRequest>(requestBody);

                var validation = new CustomerValidations().Validate(request);
                if (!validation.IsValid) return new BadRequestObjectResult(new { Errors = validation.Errors.Select(j => j.ErrorMessage) });

                return await _service.UpdateCustomerAsync(id, request);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
