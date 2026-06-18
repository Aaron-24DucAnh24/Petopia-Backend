using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Petopia.DataLayer.Interfaces
{
  public interface IBaseDataLayer<T> where T : class
  {
    public IQueryable<T> AsQueryable();
    public IIncludableQueryable<T, K> Include<K>(Expression<Func<T, K>> exp);
    public IQueryable<T> AsTracking();
    public IQueryable<T> Where(Expression<Func<T, bool>> exp);
    public int Count(Expression<Func<T, bool>> exp);
    public ValueTask<int> CountAsync(Expression<Func<T, bool>> exp);
    public T? FirstOrDefault(Expression<Func<T, bool>> exp);
    public ValueTask<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> exp);
    public ValueTask<T> FirstAsync(Expression<Func<T, bool>> exp);
    public void Delete(T entity);
    public T Create(T entity);
    public ValueTask<T> CreateAsync(T entity);
    public T Update(T entity);
    public bool Any(Expression<Func<T, bool>> exp);
    public ValueTask<bool> AnyAsync(Expression<Func<T, bool>> exp);
    public Task<List<T>> ToListAsync();
  }
}