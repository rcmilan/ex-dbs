using infra.graph;
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
                .Where((Models.Person follower) => follower.Id == followerId)
                .AndWhere((Models.Person target) => target.Id == targetId)
                .Create("(follower)-[rel:FOLLOWS $relParam]->(target)")
                    .WithParam("relParam", new { FollowDate = DateTime.Now, FollowId = Guid.NewGuid() })
                .Return((follower, target) => new { Follower = follower.As<Models.Person>(), Target = target.As<Models.Person>() })
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
            Models.Person person = new(Guid.NewGuid(), name, account);

            var results = await _graphClient
                .Cypher
                .Create("(p:Person $person)")
                .WithParam("person", person)
                .Return(p => p.As<Models.Person>())
                .ResultsAsync;

            return Ok(results);
        }

        private async Task<Models.Person?> GetById(Guid personId)
        {
            var targetAccount = await _graphClient
                .Cypher
                .Match("(p:Person)")
                .Where((Models.Person p) => p.Id == personId)
                .Return(p => p.As<Models.Person>())
                .ResultsAsync;

            return targetAccount.ToList().FirstOrDefault();
        }
    }
}