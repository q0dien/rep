using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiSecurity.Filters;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private const string ApiKeyHeader = "X-Api-Key";
    private const string ExpectedApiKey = "SecretKey123";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(
                ApiKeyHeader, out var apiKey) ||
            apiKey != ExpectedApiKey)
        {
            context.Result = new UnauthorizedObjectResult(
                new { message = "Неверный или отсутствующий API Key" });

            return;
        }

        await next();
    }
}