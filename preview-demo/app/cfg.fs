module app.cfg

open pilipala.data.db
open pilipala.builder
open pilipala.container.post
open pilipala.container.comment
open pilipala.plugin
open pilipala.util.hash
open pilipala.util.text

let getDbCfg () =
    { connection =
        {| host = "localhost"
           port = 5432us
           usr = "postgres"
           pwd = "65a1561425f744e2b541303f628963f8"
           using = "pilipala" |}
      pooling = {| size = 32us; sync = 180us |}
      map =
        {| post = "schema.post"
           comment = "schema.comment"
           token = "schema.token"
           user = "schema.user" |} }
