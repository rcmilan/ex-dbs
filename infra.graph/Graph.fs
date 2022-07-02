namespace infra.graph

open System
open Neo4jClient

[<CLIMutable>]
type Person = { Name : string; Twitter : string }

[<CLIMutable>]
type KnowsData = { Details : string }

type FollowRelationship(target) =
    inherit Relationship(target)
    interface IRelationshipAllowingSourceNode<Person>
    interface IRelationshipAllowingTargetNode<Person>

    override this.RelationshipTypeKey
        with get() = "follows"

type KnowsRelationship(target, data) =
    inherit Relationship(target, data)
    interface IRelationshipAllowingSourceNode<Person>
    interface IRelationshipAllowingTargetNode<Person>

    override this.RelationshipTypeKey
        with get() = "knows"

module Graph =
    let connString = @"http://neo4j:7474/"
    let userName = "neo4j"
    let password = "test"

    let client = new GraphClient(new Uri(connString), userName, password)

    let connect =
        client.ConnectAsync()

    let addPerson (person : Person) =

        if not client.IsConnected then
            connect
        else
            client.Cypher
                .Create("(p:Person $person)")
                .WithParam("person", person)
                .ExecuteWithoutResultsAsync()