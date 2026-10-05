using System.Text.Json;

namespace DeskFlow.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await TratarErroAsync(context, ex);
        }
    }

    private async Task TratarErroAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Erro interno não tratado.");

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var resposta = new
        {
            status = statusCode,
            erro = statusCode == StatusCodes.Status500InternalServerError
                ? "Ocorreu um erro interno no servidor."
                : exception.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
    }
}
