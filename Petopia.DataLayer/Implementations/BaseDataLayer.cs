using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Petopia.Data;
using Petopia.DataLayer.Interfaces;

namespace Petopia.DataLayer.Implementations
{
  public class BaseDataLayer<T> : IBaseDataLayer<T> where T : class
  {
    private readonly ApplicationDbContext _dbContext;
    protected DbSet<T> DbSet => _dbContext.Set<T>();

    public BaseDataLayer(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public bool Any(Expression<Func<T, bool>> exp)
    {
      return DbSet.Where(exp).Any();
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<T, bool>> exp)
    {
      return await DbSet.Where(exp).AnyAsync();
    }

    public IQueryable<T> AsQueryable()
    {
      return DbSet;
    }

    public IQueryable<T> AsTracking()
    {
      return DbSet.AsTracking();
    }

    public int Count(Expression<Func<T, bool>> exp)
    {
      return DbSet.Where(exp).Count();
    }

    public async ValueTask<int> CountAsync(Expression<Func<T, bool>> exp)
    {
      return await DbSet.Where(exp).CountAsync();
    }

    public T Create(T entity)
    {
      return DbSet.Add(entity).Entity;
    }

    public async ValueTask<T> CreateAsync(T entity)
    {
      return (await DbSet.AddAsync(entity)).Entity;
    }

    public async ValueTask CreateRangeAsync(T[] entity)
    {
      await DbSet.AddRangeAsync(entity);
    }

    public void Delete(T entity)
    {
      DbSet.Remove(entity);
    }

    public async ValueTask DeleteAllAsync(Expression<Func<T, bool>> exp)
    {
      List<T> objects = await DbSet.AsTracking().Where(exp).ToListAsync();
      foreach (var obj in objects)
      {
        DbSet.Remove(obj);
      }
    }

    public void DeleteRange(params T[] entities)
    {
      DbSet.RemoveRange(entities);
    }

    public T? FirstOrDefault(Expression<Func<T, bool>> exp)
    {
      return DbSet.Where(exp).FirstOrDefault();
    }

    public async ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> exp)
    {
      return await DbSet.Where(exp).FirstOrDefaultAsync();
    }

    public async ValueTask<T> FirstAsync(Expression<Func<T, bool>> exp)
    {
      return await DbSet.Where(exp).FirstAsync();
    }

    public IIncludableQueryable<T, K> Include<K>(Expression<Func<T, K>> exp)
    {
      return DbSet.Include(exp);
    }

    public T Update(T entity)
    {
      return DbSet.Update(entity).Entity;
    }

    public void UpdateRange(T[] entities)
    {
      DbSet.UpdateRange(entities);
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> exp)
    {
      return DbSet.Where(exp);
    }

    public async Task<List<T>> ToListAsync()
    {
      return await DbSet.ToListAsync<T>();
    }
  }
}