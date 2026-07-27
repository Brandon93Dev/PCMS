using PCMS_Backend.Extensions;
using PCMS_Backend.Models;
using PCMS_Backend.Queries;
using PCMS_Backend.Repositories;

namespace PCMS_Backend.Services;

public class ProductService
{
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> SearchAsync(ProductQuery query)
    {
        var products = await _productRepository.GetAllAsync();
        var queryable = products.AsQueryable();

        queryable = queryable
            .FilterByCategory(query.CategoryId)
            .FilterByPrice(query.MinPrice, query.MaxPrice)
            .FilterByNameOrDesc(query.Name);

        queryable = query.SortBy?.ToLower() switch
        {
            "name" => queryable.SortByName(query.SortDirection),
            "price" => queryable.SortByPrice(query.SortDirection),
            _ => queryable
        };

        queryable = queryable
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize);

        return queryable.ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        var existing = await _productRepository.GetAllAsync();
        if (existing.Any(p => p.SKU.Trim().Equals(product.SKU.Trim(), StringComparison.OrdinalIgnoreCase)))        
            throw new InvalidOperationException("A product with this name already exists.");
        
        await _productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    { 
        await _productRepository.DeleteAsync(id); 
    }
}
