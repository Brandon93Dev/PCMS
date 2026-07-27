using PCMS_Backend.Models;

namespace PCMS_Backend.Extensions;

public static class ProductExtensions
{
    //Price filter functionality
    public static IQueryable<Product> FilterByPrice(this IQueryable<Product>? query, decimal? minPrice, decimal? maxPrice)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        return query;
    }

    //Category filter functionality
    public static IQueryable<Product> FilterByCategory(this IQueryable<Product>? query, int? categoryId)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        return query;
    }

    //Name or Description filter functionality
    public static IQueryable<Product> FilterByNameOrDesc(this IQueryable<Product>? query, string? searchTerm)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p =>
                (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchTerm)) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.Contains(searchTerm)));

        return query;
    }

    //Sort by price plus indicator
    public static IQueryable<Product> SortByPrice(this IQueryable<Product>? query, SortDirection sortDirection = SortDirection.asc)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        if (sortDirection == SortDirection.desc)
            query = query.OrderByDescending(p => p.Price);
        else
            query = query.OrderBy(p => p.Price);
        return query;
    }

    //Sort By Name plus indicator
    public static IQueryable<Product> SortByName(this IQueryable<Product>? query, SortDirection sortDirection = SortDirection.asc)
    {
        if (query is null) throw new ArgumentNullException(nameof(query));

        return sortDirection == SortDirection.asc ?
            query.OrderBy(p => p.Name) :
            query.OrderByDescending(p => p.Name);
    }
}

public enum SortDirection
{
    asc,
    desc
}

