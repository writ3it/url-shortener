using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace UrlShortener.Backend.Redirecting
{
    public record CreateRedirectionRequest(string URL);

    public class AddRedirectionFunction
    {
        private readonly ILogger<AddRedirectionFunction> _logger;
        private readonly IRedirectingService _service;

        public AddRedirectionFunction(
            ILogger<AddRedirectionFunction> logger,
            IRedirectingService service
        )
        {
            _logger = logger;
            _service = service;
        }

        [Function("AddRedirection")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [FromBody] CreateRedirectionRequest request
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            if (request == null)
            {
                return new BadRequestObjectResult("Request JSON malformed.");
            }

            var redirection = _service.CreateRedirection(request.URL, Model.Type.FLEXIBLE);
            _logger.LogInformation(
                $"PK={redirection.PartitionKey} RK={redirection.RowKey} URL={redirection.URL} Type={redirection.Type}"
            );
            return new OkObjectResult(
                new { Message = "Redirection created.", Token = redirection.RowKey }
            );
        }
    }
}
