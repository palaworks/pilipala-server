module app.cfg

open System
open pilipala.data.db
open pilipala.util.io
open fsharper.alias
open pilipala.util.text

type Cfg =
    { plugins: string array
      db_name: string
      db_user: string
      db_pwd: string }

let cfg =
    { json = readFile "./config/config.json" }
        .deserializeTo<Cfg>()
        .unwrap ()

let getDbCfg () =
    { connection =
        {| host = "localhost"
           port = 5432us
           usr = cfg.db_user
           pwd = cfg.db_pwd
           using = cfg.db_name |}
      pooling = {| size = 32us; sync = 180us |}
      map =
        {| post = "schema.post"
           comment = "schema.comment"
           token = "schema.token"
           user = "schema.user" |} }
