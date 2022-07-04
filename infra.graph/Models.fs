namespace infra.graph

    module Models =
        open System
        open Neo4jClient

        [<CLIMutable>]
        type Person = { Id : Guid; Name : string; Account : string }

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