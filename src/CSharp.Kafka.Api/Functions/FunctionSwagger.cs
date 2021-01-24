using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CSharp.Kafka.Api.Functions;
using Microsoft.Azure.WebJobs.Extensions.Http;
using NSwag.SwaggerGeneration.AzureFunctionsV2;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using Aliencube.AzureFunctions.Extensions.OpenApi.Extensions;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;

namespace Cliente.Cancelamento.Batch.Functions
{
    public class FunctionSwagger
    {
        private const string TITLE = "C#, Kafka, CDC, Worker, Functions, Sql Server e Slack";

        [OpenApiIgnore]
        [FunctionName("Swagger")]
        public async Task<IActionResult> Swagger([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger/v1")] HttpRequest request)
        {
            var settings = new AzureFunctionsV2ToSwaggerGeneratorSettings { Title = TITLE };
            var generator = new AzureFunctionsV2ToSwaggerGenerator(settings);
            var document = await generator.GenerateForAzureFunctionClassesAsync(
            new List<Type>
            {
                typeof(FunctionCustomers),
                typeof(FunctionCustomerById),
                typeof(FunctionCreateCustomer),
                typeof(FunctionUpdateCustomer),
                typeof(FunctionDeleteCustomer),
            },
            new List<string>
            {
                "FunctionCustomers",
                "FunctionCustomerById",
                "FunctionCreateCustomer",
                "FunctionUpdateCustomer",
                "FunctionDeleteCustomer",
            });

            var json = document.ToJson();
            return new OkObjectResult(json);
        }

        [OpenApiIgnore]
        [FunctionName("SwaggerUI")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "swagger")] HttpRequest request)
        {
            var ui = new SwaggerUI();
            var result = await ui.AddMetadata(new OpenApiInfo { Title = TITLE })
                .AddServer(request, "")
                .BuildAsync()
                .RenderAsync("api/swagger/v1")
                .ConfigureAwait(false);

            var response = new ContentResult()
            {
                Content = result,
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }
    }
}
