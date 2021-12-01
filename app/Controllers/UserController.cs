using infra.cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using model.Entities;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ICacheService<User, Guid> _cacheService;

        public UserController(ICacheService<User, Guid> cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpPost]
        public Task<User> Post(User user)
        {
            return _cacheService.Save(user);
        }
    }
}
