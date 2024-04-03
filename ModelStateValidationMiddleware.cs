using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace WebApi;

/// <summary>
/// 
/// </summary>
public class ModelStateValidationMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    public ModelStateValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/v1")) // Adjust the path as needed
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata.GetMetadata<ApiControllerAttribute>() != null)
            {
                if (context.Items["__MODEL_STATE"] is ModelStateDictionary { IsValid: false } modelState)
                {
                    var errors = modelState.Values.SelectMany(v => v.Errors);
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { response = errors });
                    return;
                }
            }
        }

        await _next(context).ConfigureAwait(false);
    }
}