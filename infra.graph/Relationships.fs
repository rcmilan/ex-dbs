namespace infra.graph.relationships

open System
open infra.graph.models

type FollowInfo = { Id : Guid; Date : DateTime }

type FollowerRelationship = { Target : Person; Info : FollowInfo; Follower : Person }