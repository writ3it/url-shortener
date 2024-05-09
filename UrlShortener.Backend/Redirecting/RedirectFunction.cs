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

        public RedirectFunction(ILogger<RedirectFunction> logger)
        {
            _logger = logger;
        }

        [Function("Redirect")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "redirect/{token:alpha}")]
                HttpRequest req,
            string token,
            [TableInput("redirection","all","{token}")]
            IEnumerable<Redirection> redirections
        )
        {
            if (!redirections.Any())
            {
                _logger.LogInformation("Token not found. Token: " + token);
                return new RedirectResult("https://zhp.pl", false, true);
            }
            var redirection = redirections.First();
            _logger.LogInformation("Token found. Token: " + token);
            return new RedirectResult(redirection.URL, redirection.IsPernament, redirection.IsPreservingMethod);
        }

    }
}
