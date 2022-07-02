using model.Entities;

namespace infra.cache
{
    public interface ICacheRepository<T, ID> where T : BaseEntity<ID>
    {
        Task<T> Add(T entity);

        Task<T> Get(ID key);
    }
}