using System.Linq.Expressions;
using SistemaClinica.Domain.Entities;

namespace SistemaClinica.Application.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntitie
{
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includes);
    Task<List<TEntity>> SearchAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        int skip,
        int take,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includes);
    Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task SalveAsync(CancellationToken cancellationToken);
}
