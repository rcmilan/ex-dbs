namespace infra.graph.service

open infra.graph.models

open System.Collections.Generic
open System
open Neo4jClient
open System.Threading.Tasks

type IMyGraphService =
    abstract member Get : id:Guid -> Task<IEnumerable<Person>>
    abstract member Create : p:Person -> IEnumerable<Person>

type MyGraphService (cypherGraphClient : ICypherGraphClient) =
    interface IMyGraphService with
        member this.Create(p: Person): IEnumerable<Person> = 
            raise (System.NotImplementedException())

        member this.Get(id: Guid): Task<IEnumerable<Person>> = 
            cypherGraphClient
                .Cypher
                .Match("(p:Person)")
                .Where(fun (p:Person) -> p.Id = id)
                .Return<Person>("p")
                .ResultsAsync