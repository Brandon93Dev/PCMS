using Microsoft.EntityFrameworkCore;
using PCMS_Backend.Infrastructure.Data;
using PCMS_Backend.Models;
using PCMS_Backend.Services;

namespace PCMS_Backend.Repositories;

//using reposiutory base to implement all basic expected crud logic
public class ProductRepository :RepositoryBase<Product>, IProductRepository
{
    private readonly ProductSearchCache _cache;

    public ProductRepository(AppDbContext dbContext, ProductSearchCache cache) : base(dbContext) 
    {
        _cache = cache;
    }
        
    public async Task<IEnumerable<Product>> SearchByNameAsync(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return Enumerable.Empty<Product>();

        if (_cache.TryGet(keyword, out var product))
            return product;

        var result = await _dbSet
            .Where(p => 
                p.Name != null && 
                p.Name.Contains(keyword))
            .ToListAsync();

        if(result.Any())
           _cache.Store(keyword, result);

        return result;
    }
}
