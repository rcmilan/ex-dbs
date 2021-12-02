using model.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace infra.cache
{
    public class UserCacheService : ICacheService<User, Guid>
    {
        private readonly IConnectionMultiplexer _connMultiplexer;
        private readonly IDatabase _cacheDb;

        public UserCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connMultiplexer = connectionMultiplexer;

            _cacheDb = _connMultiplexer.GetDatabase();
        }

        public async Task<User> Get(Guid key)
        {
            var res = await _cacheDb.StringGetAsync(key.ToString());

            return res.IsNull ? default : JsonConvert.DeserializeObject<User>(res);
        }

        public async Task<User> Save(User user)
        {
            await _cacheDb.StringSetAsync(user.ID.ToString(), JsonConvert.SerializeObject(user));

            return user;
        }
    }
}