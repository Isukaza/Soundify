using Soundify.DAL.PostgreSQL.Models.db.Base;

namespace Soundify.DAL.PostgreSQL.Repository.Interfaces.Base;

public interface IDbRepositoryBase<T>
    where T : BaseDbEntity
{
    T Create(T entity);

    bool Update(T entity);

    bool Delete(T entity);

    Task<T> CreateAsync(T entity);

    Task<bool> UpdateAsync(T entity);

    Task<bool> DeleteAsync(T entity);

    Task<bool> SaveAsync();
}