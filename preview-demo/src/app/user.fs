module app.user

open System
open pilipala.builder
open pilipala.plugin
open NReco.Logging.File
open fsharper.op
open fsharper.op.Foldable
open fsharper.typ
open app.cfg

let App () =
    Builder
        .make()
        .useDb(getDbCfg ())
        .apply(cfg.plugins.foldl <| fun b -> b.usePlugin)
        .useLoggerProvider(new FileLoggerProvider("./pilipala.log"))
        .build ()

//TODO caseless username

//Display
let pl_display_user =
    App()
        .UserLogin(cfg.pl_comment_user, cfg.pl_comment_pwd)
        .unwrap ()

//Anonymous
let pl_comment_user =
    App()
        .UserLogin(cfg.pl_comment_user, cfg.pl_comment_pwd)
        .unwrap ()
