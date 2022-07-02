namespace infra.graph

module Graph =
    open System
    open Neo4jClient

    open Types

    [<Literal>]
    let connString = @"http://neo4j:7474/"
    let userName = "neo4j"
    let password = "test"

    let neo4jClient = new GraphClient(new Uri(connString), userName, password)

    let nodeLabel node =
        match node.Label with
            | START -> Types.Label.START
            | END -> Types.Label.END
            | ENTRY -> Types.Label.ENTRY
            | RETRY -> Types.Label.RETRY
//            | RESPONSE -> Types.Label.RESPONSE //NOT yet a part of the schema

    let relationshipType rel =
        match rel with
            | GOTO -> Types.Relationship.GOTO
            | FAIL -> Types.Relationship.FAIL
            | SUCCESS -> Types.Relationship.SUCCESS

    let cypherMergeNode label alias =
        sprintf "(%s:%s {id: {id}, title: {title}, message: {message}, retries: {retries} })" alias label

    let cypherGetNode node alias =
        sprintf "(%s:%s)" (nodeLabel node) alias

    let createNode (node:IVRNode) =
        neo4jClient.ConnectAsync()

        let inline (=>) a b = a, box b
        let nodeProperties =  node.Properties

        neo4jClient.Cypher
            .Merge(cypherMergeNode (nodeLabel node) "a")
            .OnCreate()
            .Set("a = {nodeProperties}")
            .WithParams(dict [
                            "id" => nodeProperties.id
                            "title" => nodeProperties.title
                            "message" => nodeProperties.message
                            "retries" => nodeProperties.retries
                            "nodeProperties" => nodeProperties
                        ])
            .ExecuteWithoutResultsAsync()

    let deleteNode (node:IVRNode) =
        neo4jClient.Cypher
            .OptionalMatch(sprintf "(n:%s)<-[r]-()" (nodeLabel node))
            .Where(fun (n:NodeProperties) -> n.id = node.Properties.id)
            .Delete("r, n") //deletes all incoming relationships
            .ExecuteWithoutResultsAsync()

    let createRelationship (path:IVRPath) =
        let node1, rel, node2 = path

        neo4jClient.Cypher
            .Match(cypherGetNode node1 "n1", cypherGetNode node2 "n2")
            .Where(fun (n1:NodeProperties) -> n1.id = node1.Properties.id)
            .AndWhere(fun (n2:NodeProperties) -> n2.id = node2.Properties.id)
            .CreateUnique(sprintf "n1-[:%s]-n2" (relationshipType rel))
            .ExecuteWithoutResultsAsync()