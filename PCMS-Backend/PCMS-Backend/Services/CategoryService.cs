using PCMS_Backend.Models;
using PCMS_Backend.Repositories;

namespace PCMS_Backend.Services;

public class CategoryService
{
    private readonly IRepository<Category> _categoryRepository;

    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    #region CRUD based opperations
    public async Task AddAsync(Category category) 
    { 
       await _categoryRepository.AddAsync(category);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync() 
    {
       return await _categoryRepository.GetAllAsync();
    }   

    public async Task UpdateAsync(Category category) {
        await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id) 
    { 
        await _categoryRepository.DeleteAsync(id);
    }
    #endregion

    //We saw that were able to create duplicate categories, added this for deduping
    public async Task<bool> ExistsAsync(string name, int? parentCategoryId)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Any(c =>
            c.Name.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) &&
            c.ParentCategoryId == parentCategoryId
        );
    }

    //Get hieracial tree view of categories
    public async Task<IEnumerable<Category>> GetTreeAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var lookup = categories.ToLookup(c => c.ParentCategoryId);

        foreach (var category in categories)        
            category.SubCategories = lookup[category.Id].ToList();        
       
        return categories.Where(c => c.ParentCategoryId == null || c.ParentCategoryId == -99);
    }
}
