using infra.cache;
using Microsoft.AspNetCore.Mvc;
using model.Entities;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ICacheRepository<User, Guid> _repository;

        public UserController(ICacheRepository<User, Guid> cacheService)
        {
            _repository = cacheService;
        }

        [HttpPost]
        public Task<User> Post(User user)
        {
            return _repository.Add(user);
        }

        [HttpGet]
        public Task<User> Get(Guid ID)
        {
            return _repository.Get(ID);
        }
    }
}
