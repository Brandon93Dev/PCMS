using Microsoft.EntityFrameworkCore;    
using PCMS_Backend.Infrastructure.Data;
using PCMS_Backend.Models;
using PCMS_Backend.Repositories;
using PCMS_Backend.Services;

namespace PCMS_Backend.Infrastructure;

public static class DependencyInjection
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"), 
            new MySqlServerVersion(new Version(5, 7, 18))
        ));

        //init product search cache
        services.AddSingleton<ProductSearchCache>();

        // Repositories.. product is EFcore ..Category is InMemory
        services.AddScoped<IRepository<Product>, ProductRepository>();

        //In memory collection
        services.AddSingleton<IRepository<Category>, CategoryRepository>();

        // Services
        services.AddScoped<ProductService>();
        services.AddScoped<CategoryService>();

        //Used to store in memory items
        services.AddMemoryCache();  
    }
}
