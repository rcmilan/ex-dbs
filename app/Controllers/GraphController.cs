using infra.graph;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly GraphClient _graphClient;

        public GraphController(GraphDbClient graphDbClient)
        {
            this._graphClient = graphDbClient.Client;
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow(Guid targetId, Guid followerId)
        {
            var target = await Get(targetId);
            var follower = await Get(followerId);

            await _graphClient
                .Cypher
                .Merge("(target:Person $target)").WithParam("target", target)
                .Merge("(follower:Person $follower)").WithParam("follower", follower)
                .Create("(follower)-[r: Follows]->(target)")
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string name, string account)
        {
            Models.Person person = new(Guid.NewGuid(), name, account);

            await _graphClient
                .Cypher
                .Create("(p:Person $person)")
                .WithParam("person", person)
                .ExecuteWithoutResultsAsync();

            return Ok(person);
        }

        [HttpGet("{personId}")]
        private async Task<IActionResult> Get(Guid personId)
        {
            var result = await GetById(personId);

            return Ok(result);
        }

        private async Task<Models.Person?> GetById(Guid personId)
        {
            var targetAccount = await _graphClient.Cypher.Match("(p:Person)")
                            .Where((Models.Person p) => p.Id.Equals(personId))
                            .Return(p => p.As<Models.Person>())
                            .ResultsAsync;

            return targetAccount.ToList().FirstOrDefault();
        }
    }
}