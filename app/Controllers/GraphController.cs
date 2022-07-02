using infra.graph;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var neo4jClient = Graph.neo4jClient;

            await neo4jClient.ConnectAsync();

            return Ok();
        }
    }
}
