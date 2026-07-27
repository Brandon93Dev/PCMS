using PCMS_Backend.Models;

namespace PCMS_Backend.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    //Create
    public Task AddAsync(Category entity)
    {
        if (entity.Id == 0)        
            entity.Id = _nextId++;
        
        _categories.Add(entity);
        return Task.CompletedTask;
    }

    //Read
    public Task<Category?> GetByIdAsync(int id)
    {
        return Task.FromResult(
            _categories.FirstOrDefault(c => c.Id == id));
    }

    //Read all
    public Task<IEnumerable<Category>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Category>>(_categories);
    }

    //Update
    public Task UpdateAsync(Category entity)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == entity.Id);
        if(existing != null)
        {
            _categories.Remove(existing);
            _categories.Add(entity);
        }
        return Task.CompletedTask;
    }

    //Delete (no soft delete)
    public Task DeleteAsync(int id)
    {
        var existing = _categories.FirstOrDefault(c => c.Id == id);
        if (existing is not null)
        {
            _categories.Remove(existing);
        }
        return Task.CompletedTask;
    }


    public Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategooryId)
    {
       return Task.FromResult(
           _categories.Where(c => c.ParentCategoryId == parentCategooryId));
    }
}
