using infra.document;
using Microsoft.AspNetCore.Mvc;
using model.Entities;
using MongoDB.Bson;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentRepository<Document, ObjectId> _repository;

        public DocumentController(IDocumentRepository<Document, ObjectId> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Document document)
        {
            var id = await _repository.Add(document);

            return new JsonResult(id.ToString());
        }

        [HttpGet]
        public Task<IEnumerable<Document>> Get()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}")]
        public Task<Document> Get(string id)
        {
            return _repository.Get(id);
        }
    }
}