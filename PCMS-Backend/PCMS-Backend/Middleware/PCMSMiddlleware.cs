
using System.Net;

namespace PCMS_Backend.Middleware;

public class PCMSMiddlleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PCMSMiddlleware> _logger;

    public PCMSMiddlleware(RequestDelegate next, ILogger<PCMSMiddlleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            DateTime dateTime = DateTime.Now;
            IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

            //Request logging done by cujstomised middleware to log where requests came from and what time the requests were made
            _logger.LogInformation("Incoming request at {Timestamp} from IP :{IpAddress}", 
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                remoteIpAddress);

            await _next(context);

            _logger.LogInformation("Response sent at {Timestamp} for request from IP :{IpAddress}", 
                dateTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                remoteIpAddress);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    }
}

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PCMSMiddlleware>();
    }
}

