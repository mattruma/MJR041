using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IEntityDataStore<TKey, TEntity> where TEntity : IEntity<TKey>
    {
        Task AddAsync(
            TEntity entity);

        Task DeleteByIdAsync(
            TKey id);

        Task<TEntity> GetByIdAsync(
            TKey id);

        Task UpdateAsync(
            TEntity entity);
    }
}