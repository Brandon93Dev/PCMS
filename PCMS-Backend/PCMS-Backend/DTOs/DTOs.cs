namespace PCMS_Backend.DTOs
{
    public record ProductDto(
        int Id,
        string? Name,
        string? Description,
        string? SKU,
        decimal Price,
        decimal Quantity,
        int CategoryId,
        DateTime CreatedAt,
        DateTime ModifiedAt
    );


    public record CategoryDto(
        int Id,
        string? Name,
        string? Description,
        int? ParentCategoryId,
        List<CategoryDto>? SubCategories = null
    );
}