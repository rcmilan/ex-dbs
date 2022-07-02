using infra.graph;
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
            var label = Types.Label.START;
            var node = new Types.IVRNode(label, new Types.NodeProperties(1, "title", "msg", 0));

            Graph.createNode(node).Wait();

            return Ok();
        }
    }
}