using System.Linq.Expressions;

namespace CoursesManager.Application.Abstractions.Persistence
{

    // Interface som säger vilka metoder ett repository måste ha.
    // Gör det enkelt att byta databas utan att ändra i resten av koden.
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken ct = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> findBy);

        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> where, CancellationToken ct = default);

        Task<TEntity?> GetOneAsync(
        Expression<Func<TEntity, bool>> where,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

        Task<TSelect?> GetOneAsync<TSelect>(
        Expression<Func<TEntity, bool>> where,
        Expression<Func<TEntity, TSelect>> select,
        bool tracking = false,
        CancellationToken ct = default);


        Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

        Task<IReadOnlyList<TSelect>> GetAllAsync<TSelect>(
        Expression<Func<TEntity, TSelect>> select,
        Expression<Func<TEntity, bool>>? where = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        bool tracking = false,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);


        Task DeleteAsync(TEntity entity, CancellationToken ct = default);

        Task<int> SaveChangesAsync(CancellationToken ct = default);

    }
}