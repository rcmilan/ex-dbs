using model.Entities;

namespace infra.embed
{
    public class EmbedService<T, TId> : IEmbedRepository<T, TId> where T : BaseEntity<TId>
    {
        private readonly EmbedContext _context;

        public EmbedService(EmbedContext context)
        {
            _context = context;
        }

        public IEnumerable<T> Get()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public T Get(TId Id)
        {
            return _context.Set<T>().Find(Id)!;
        }
    }
}