using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;
using System.Linq.Expressions;

namespace Repositories.Repos;

public abstract class RepositoryBase<T>
    (RepositoryContext repositoryContext)
    : IRepositoryBase<T>
    where T : class
{
    protected DbSet<T> table = repositoryContext.Set<T>();

    public void Create(T entity)
        => table.Add(entity);

    public void Delete(T entity)
        => table.Remove(entity);

    public IQueryable<T> FindAll(bool changeTracker)
        => !changeTracker ? table.AsNoTracking() :
                            table;


    public IQueryable<T> FindCondition(Expression<Func<T, bool>> expression, bool changeTracker)
        => !changeTracker ? table.Where(expression).AsNoTracking() :
                            table.Where(expression);

    public void Update(T entity)
        => table.Update(entity);
}
