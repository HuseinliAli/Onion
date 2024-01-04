using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool changeTracker);
    IQueryable<T> FindCondition(Expression<Func<T, bool>> expression, bool changeTracker);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
