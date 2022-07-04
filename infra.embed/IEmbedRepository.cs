using model.Entities;

namespace infra.embed
{
    public interface IEmbedRepository<T, TId> where T : BaseEntity<TId>
    {
        IEnumerable<T> Get();

        T Get(TId Id);
    }
}