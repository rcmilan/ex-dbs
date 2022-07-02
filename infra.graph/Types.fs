﻿module Types

type Label =
    | START
    | END
    | ENTRY
    | RETRY
    | RESPONSE

type Relationship =
    | GOTO
    | SUCCESS
    | FAIL

type NodeProperties =
    {
        id: int
        title: string
        message: string
        retries: int

    }

type IVRNode =
    {
        Label: Label
        Properties: NodeProperties
    }

type IVRPath = IVRNode * Relationship * IVRNode // Exemplo: --> -->(ENTRY)-[:FAIL]->(RETRY)

type Pair = IVRNode * IVRNode