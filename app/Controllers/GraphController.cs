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
            await _graphClient.Cypher
                .Match("(follower:Person)", "(target:Person)")
                .Where((Models.Person follower) => follower.Id == followerId)
                .AndWhere((Models.Person target) => target.Id == targetId)
                .CreateUnique("(follower)-[rel:FOLLOWS]->(target)")
                .ExecuteWithoutResultsAsync();

            return Ok();
        }

        [HttpGet("{personId}")]
        public async Task<IActionResult> Get(Guid personId)
        {
            var result = await GetById(personId);

            return Ok(result);
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
                            .Where((Models.Person p) => p.Id == personId)
                            .Return(p => p.As<Models.Person>())
                            .ResultsAsync;

            return targetAccount.ToList().FirstOrDefault();
        }
    }
}