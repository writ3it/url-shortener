using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

using UrlShortener.Backend.Redirecting.Model;

namespace UrlShortener.Backend.Redirecting
{
    public class RedirectFunction
    {
        private readonly ILogger<RedirectFunction> _logger;
        private readonly IRedirectingService _service;

        public RedirectFunction(ILogger<RedirectFunction> logger, IRedirectingService service)
        {
            _logger = logger;
            _service = service;
        }

        [Function("Redirect")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "redirect/{token:regex(^[0-9A-Za-z]{{3,40}}$)}")]
                HttpRequest req,
            string token
            /**
            Is it possible?
            [TableInput("redirection","all","{token}")] <--- token
            IEnumerable<Redirection> redirections
            **/
        )
        {
            var redirection = _service.FindByRowKey(token);
            if (redirection == null)
            {
                _logger.LogInformation("Token not found. Token: " + token);
                return new RedirectResult("https://zhp.pl", false, true);
            }
            _logger.LogInformation("Token found. Token: " + token);
            return new RedirectResult(redirection.URL, redirection.IsPernament, redirection.IsPreservingMethod);
        }

    }
}
