namespace infra.graph

open System
open Neo4jClient

type GraphDbClient(connString : Uri, user : string, password : string) = 
    let client = new GraphClient(connString, user, password)

    member this.Client = client;
