using PCMS_Backend.Models;

namespace PCMS_Backend.Services;

public class ProductSearchCache
{
    private readonly Dictionary<string, IEnumerable<Product>> _cache = new();

    public bool TryGet(string keyword, out IEnumerable<Product>? rows)
    {
        return _cache.TryGetValue(keyword, out rows);
    }

    public void Store(string keyword, IEnumerable<Product>? rows) 
    {
        _cache[keyword] = rows;
    }
}
