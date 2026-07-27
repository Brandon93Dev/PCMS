using PCMS_Backend.Models;

namespace PCMS_Backend.Repositories;

public interface ICategoryRepository: IRepository<Category>
{
    Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentCategooryId);
}
