namespace infra.graph

module Graph = 
    open System
    open Types
    open Neo4jClient
    
    let connString =  @"http://neo4j:7474"
    let userName = "neo4j"
    let password = "test"

    let neo4jClient = new GraphClient(new Uri(connString), userName, password)

    let cypherNode label alias =
        sprintf "(%s:%s {id: {id}, title: {title}, message: {message}, retries: {retries} })" alias label

    let createNode (node : IVRNode) (alias : string) = 
        let label =
            match node.Label with
                | START -> "START"
                | END -> "END"
                | ENTRY -> "ENTRY"
                | RETRY -> "RETRY"
                | RESPONSE -> "RESPONSE"

        neo4jClient.ConnectAsync()

        let inline (=>) a b = a, box b
        let nodeProperties =  node.Properties

        neo4jClient.Cypher
            .Merge(cypherNode label alias)
            .OnCreate()
            .Set("a = {nodeProperties}")
            .WithParams(dict [
                            "id" => nodeProperties.Id
                            "title" => nodeProperties.Title
                            "message" => nodeProperties.Message
                            "retries" => nodeProperties.Retries
                            "nodeProperties" => nodeProperties
                        ])
            .ExecuteWithoutResultsAsync()
