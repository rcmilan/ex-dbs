namespace infra.graph.models

open System

[<CLIMutable>]
type Person = { Id : Guid; Name : string; Account : string }