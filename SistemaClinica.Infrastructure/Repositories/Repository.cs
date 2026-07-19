using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SistemaClinica.Application.Interfaces;
using SistemaClinica.Domain.Entities;
using SistemaClinica.Infrastructure.Context;

namespace SistemaClinica.Infrastructure.Repositories;

public class Repository<TEntity>(SistemaClinicaDbContext context) : IRepository<TEntity> where TEntity : BaseEntitie
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _dbSet.OrderBy(x => x.Id).ToListAsync(cancellationToken);
    }

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return AplicarIncludes(_dbSet.AsQueryable(), includes)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public Task<List<TEntity>> SearchAsync(
        Expression<Func<TEntity, bool>>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        int skip,
        int take,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (predicate is not null)
            query = query.Where(predicate);

        query = AplicarIncludes(query, includes);
        query = orderBy is null ? query.OrderBy(x => x.Id) : orderBy(query);

        return query.Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate, CancellationToken cancellationToken)
    {
        return predicate is null
            ? _dbSet.CountAsync(cancellationToken)
            : _dbSet.CountAsync(predicate, cancellationToken);
    }

    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return _dbSet.AnyAsync(predicate, cancellationToken);
    }

    public Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        return _dbSet.AddAsync(entity, cancellationToken).AsTask();
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public Task SalveAsync(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<TEntity> AplicarIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }
}
