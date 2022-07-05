namespace infra.graph

    module Models =
        open System

        [<CLIMutable>]
        type Person = { Id : Guid; Name : string; Account : string }