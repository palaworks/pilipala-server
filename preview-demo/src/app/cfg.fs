module app.cfg

open pilipala.data.db
open pilipala.builder
open pilipala.container.post
open pilipala.container.comment
open pilipala.plugin
open pilipala.util.io
open pilipala.util.hash
open fsharper.alias
open pilipala.util.text

type Cfg =
    { pl_user: string
      pl_pwd: string
      db_name: string
      db_user: string
      db_pwd: string
      ws_local_port: i32
      ws_public_port: i32
      ws_public_ssl_enable: bool
      ws_cert_pem_path: string
      ws_cert_key_path: string }

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
