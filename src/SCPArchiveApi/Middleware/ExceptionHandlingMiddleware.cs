// Middleware pour la gestion globale des exceptions dans l'API
// Permet de capturer toutes les exceptions non gérées et de retourner une réponse JSON standardisée

using System.Text.Json;

namespace SCPArchiveApi.Middleware;

/// <summary>
/// Middleware pour la gestion globale des exceptions
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Intercepte chaque requête HTTP et capture les exceptions non gérées
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Passe au middleware suivant
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Une erreur non gérée est survenue");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Formate la réponse d'erreur en JSON selon le type d'exception
    /// </summary>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            error = new
            {
                message = "Une erreur est survenue lors du traitement de la requête",
                details = exception.Message
            }
        };

        // Code HTTP selon le type d'exception
        context.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
