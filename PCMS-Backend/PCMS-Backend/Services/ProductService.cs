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

        // Always filter by category and price. Do not apply the simple Contains-based
        // name/description filter when a name query is provided because fuzzy search
        // will be used instead. Applying the Contains filter first prevents fuzzy
        // matches from being considered 
        queryable = queryable
            .FilterByCategory(query.CategoryId)
            .FilterByPrice(query.MinPrice, query.MaxPrice);

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            // Use configured weights for overall scoring
            var engine = new ProductSearchEngine(
                SearchWeightConfig.ProductWeights,
                _productRepository
            );

            var fuzzyResults = await engine.SearchAsync(query.Name);

            var matchedIds = fuzzyResults
                .Where(r => r.Score > 0.3)
                .Select(r => r.Item.Id)
                .ToHashSet();

            queryable = queryable.Where(p => matchedIds.Contains(p.Id));
        }

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
