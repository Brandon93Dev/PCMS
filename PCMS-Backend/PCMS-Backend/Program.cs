using PCMS_Backend.Infrastructure;
using PCMS_Backend.Middleware;

namespace PCMS_Backend;

public static class Program
{
    public static void Main(string[] args)
    {
        var wab = WebApplication.CreateBuilder(args);

        // we stored cors polivy in appsettings 
        var allowedOrigins = wab.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        wab.Services.AddCors(options =>
        {
            options.AddPolicy("AllowConfiguredOrigins", builder =>
                builder.WithOrigins(allowedOrigins!)
                       .AllowAnyHeader()
                       .AllowAnyMethod());
        });

        //Service registrations
        wab.Services.AddControllers();
        wab.Services.AddEndpointsApiExplorer();
        wab.Services.AddSwaggerGen();

        DependencyInjection.ConfigureServices(wab.Services, wab.Configuration);

        var app = wab.Build();

        //Ensure http request pipeline is only confgured in dev and not in production
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //instruct app to run custom middleware
        app.UseCustomMiddleware();

        app.UseCors("AllowConfiguredOrigins");
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}