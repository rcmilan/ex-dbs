using infra.graph.models;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly ICypherGraphClient _graphClient;

        public GraphController(ICypherGraphClient graphClient)
        {
            this._graphClient = graphClient;
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow(Guid targetId, Guid followerId)
        {
            var results = await _graphClient
                .Cypher
                .Match("(follower:Person)", "(target:Person)")
                .Where((Person follower) => follower.Id == followerId)
                    .AndWhere((Person target) => target.Id == targetId)
                .Create("(follower)-[rel:FOLLOWS $relParam]->(target)")
                    .WithParam("relParam", new { FollowDate = DateTime.Now, FollowId = Guid.NewGuid() })
                .Return((follower, target) => new { Follower = follower.As<Person>(), Target = target.As<Person>() })
                .ResultsAsync;

            return Ok(results);
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
            Person person = new(Guid.NewGuid(), name, account);

            var results = await _graphClient
                .Cypher
                .Create("(p:Person $person)")
                    .WithParam("person", person)
                .Return(p => p.As<Person>())
                .ResultsAsync;

            return Ok(results);
        }

        private async Task<Person?> GetById(Guid personId)
        {
            var targetAccount = await _graphClient
                .Cypher
                .Match("(p:Person)")
                .Where((Person p) => p.Id == personId)
                .Return(p => p.As<Person>())
                .ResultsAsync;

            return targetAccount.ToList().FirstOrDefault();
        }
    }
}