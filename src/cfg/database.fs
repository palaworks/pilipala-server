namespace app.cfg.database

open pilipala.data.db
open fsharper.alias

type ConnectionCfg() =
    member val host = "" with get, set
    member val port = u16 0 with get, set
    member val user = "" with get, set
    member val password = "" with get, set

type DefinitionCfg() =
    member val name = "" with get, set
    member val table_user = "" with get, set
    member val table_post = "" with get, set
    member val table_comment = "" with get, set

type PerformanceCfg() =
    member val pooling = u16 0 with get, set

type DatabaseCfg() =
    member val connection = ConnectionCfg() with get, set
    member val definition = DefinitionCfg() with get, set
    member val performance = PerformanceCfg() with get, set

[<AutoOpen>]
module ext_DbConfig =
    type DbConfig with
        static member from(cfg: DatabaseCfg) =
            { connection =
                {| host = cfg.connection.host
                   port = cfg.connection.port
                   usr = cfg.connection.user
                   pwd = cfg.connection.password |}
              definition =
                {| name = cfg.definition.name
                   user = cfg.definition.table_user
                   post = cfg.definition.table_post
                   comment = cfg.definition.table_comment |}
              performance = {| pooling = cfg.performance.pooling |} }
