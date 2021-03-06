using infra.graph.models;
using infra.graph.relationships;
using infra.graph.service;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly ICypherGraphClient _graphClient;
        private readonly IMyGraphService _graphService;

        public GraphController(ICypherGraphClient graphClient, IMyGraphService graphService)
        {
            _graphClient = graphClient;
            _graphService = graphService;
        }

        [HttpPost("follow")]
        public async Task<IActionResult> Follow(Guid targetId, Guid followerId)
        {
            var results = await _graphClient
                .Cypher
                .Match("(follower:Person)", "(target:Person)")
                .Where((Person follower) => follower.Id == followerId)
                    .AndWhere((Person target) => target.Id == targetId)
                .Create("(follower)-[rel:Follows $relParam]->(target)")
                    .WithParam("relParam", new FollowInfo(Guid.NewGuid(), DateTime.Now))
                .Return((follower, rel, target) => new FollowerRelationship(target.As<Person>(), rel.As<FollowInfo>(), follower.As<Person>()))
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
            var targetAccount = await _graphService.Get(personId);

            return targetAccount.ToList().FirstOrDefault();
        }
    }
}