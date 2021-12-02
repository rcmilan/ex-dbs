using model.Entities;

namespace infra.document
{
    public interface IDocumentRepository<T, ID> where T : BaseEntity<ID>
    {
        Task<ID> Add(T entity);
        Task<T> Get(string id);
        Task<IEnumerable<T>> GetAll();
    }
}
