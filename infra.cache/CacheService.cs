using model.Entities;

namespace infra.cache
{
    public class CacheService : ICacheService<User, Guid>
    {
        public async Task<User> Save(User user)
        {
            return user;
        }
    }
}