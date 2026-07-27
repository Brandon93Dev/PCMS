using PCMS_Backend.Extensions;

namespace PCMS_Backend.Queries;

public class ProductQuery
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; } 
    public SortDirection SortDirection { get; set; } = SortDirection.asc; 
}
