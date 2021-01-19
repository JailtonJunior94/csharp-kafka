using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NSwag.Annotations;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CSharp.Kafka.Business.Domain.Dtos;
using NSwag.Annotations.AzureFunctionsV2;
using Microsoft.Azure.WebJobs.Extensions.Http;
using CSharp.Kafka.Business.Application.Interfaces;
using CSharp.Kafka.Business.Application.Validations;

namespace CSharp.Kafka.Api.Functions
{
    public class FunctionCreateUser
    {
        private readonly ICustomerService _service;
        private readonly ILogger<FunctionCreateUser> _logger;

        public FunctionCreateUser(ICustomerService service, ILogger<FunctionCreateUser> logger)
        {
            _logger = logger;
            _service = service;
        }

        [SwaggerRequestBodyType(typeof(CustomerRequest))]
        [SwaggerResponse(200, typeof(CustomerResponse))]
        [FunctionName("FunctionCreateUser")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/customers")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var request = JsonConvert.DeserializeObject<CustomerRequest>(requestBody);

                var validation = new CustomerValidations().Validate(request);
                if (!validation.IsValid) return new BadRequestObjectResult(new { Errors = validation.Errors.Select(j => j.ErrorMessage) });

                return await _service.CreateUserAsync(request);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception?.InnerException?.Message ?? exception?.Message}");
                return new ObjectResult(new { Errors = "Ocorreu um erro inesperado!" }) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
    }
}
