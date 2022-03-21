using infra.embed;
using Microsoft.AspNetCore.Mvc;
using model.Entities;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmbedController : ControllerBase
    {
        private readonly IEmbedRepository<Subscriber, Guid> _repository;

        public EmbedController(IEmbedRepository<Subscriber, Guid> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var result = _repository.Get();

            return Ok(result);
        }
    }
}
