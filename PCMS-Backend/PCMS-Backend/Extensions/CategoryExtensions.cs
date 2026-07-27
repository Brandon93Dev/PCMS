using PCMS_Backend.DTOs;
using PCMS_Backend.Models;

namespace PCMS_Backend.Extensions;

public class CategoryExtensions
{
    public CategoryDto MapToDto(Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name ?? string.Empty,
            category.Description ?? string.Empty,
            category.ParentCategoryId,
            category.SubCategories?.Select(MapToDto).ToList()
        );
    }
}
