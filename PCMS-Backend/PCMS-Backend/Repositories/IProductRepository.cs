using PCMS_Backend.Models;

namespace PCMS_Backend.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> SearchByNameAsync(string keyword);
}
