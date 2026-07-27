using Microsoft.EntityFrameworkCore;
using PCMS_Backend.Infrastructure.Data;

namespace PCMS_Backend.Repositories;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<T>();
    }

    //Create
    public async Task AddAsync(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    //Read
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    //Read All
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    //Update
    public async Task UpdateAsync(T entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    //Delete
    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is not null)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
