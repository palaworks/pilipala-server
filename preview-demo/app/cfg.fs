module app.cfg

open pilipala.data.db
open pilipala.builder
open pilipala.container.post
open pilipala.container.comment
open pilipala.plugin
open pilipala.util.io
open pilipala.util.hash
open pilipala.util.text

type Cfg =
    { user: string
      pwd: string
      db_name: string
      db_user: string
      db_pwd: string }

let cfg =
    { json = readFile "./config/config.json" }
        .deserializeTo<Cfg> ()

let getDbCfg () =
    { connection =
        {| host = "localhost"
           port = 5432us
           usr = cfg.db_user
           pwd = cfg.pwd
           using = cfg.db_name |}
      pooling = {| size = 32us; sync = 180us |}
      map =
        {| post = "schema.post"
           comment = "schema.comment"
           token = "schema.token"
           user = "schema.user" |} }
