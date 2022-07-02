using infra.graph;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        public GraphController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            Person x = new("name", "account");

            await Graph.addPerson(x);

            return Ok();
        }
    }
}