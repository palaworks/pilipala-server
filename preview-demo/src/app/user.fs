module app.user

open pilipala.builder
open pilipala.plugin
open NReco.Logging.File
open app.cfg

let App () =
    Builder
        .make()
        .useDb(getDbCfg ())
        .usePlugin("./plugin/Markdown")
        .usePlugin("./plugin/PostCover")
        .usePlugin("./plugin/PostStatus")
        .usePlugin("./plugin/Topics")
        .usePlugin("./plugin/ViewCount")
        .usePlugin("./plugin/Summarizer")
        .usePlugin("./plugin/PartialOrder")
        .usePlugin("./plugin/UserName")
        .usePlugin("./plugin/UserAvatarUrl")
        .usePlugin("./plugin/UserSiteUrl")
        .usePlugin("./plugin/Cacher")
        .useLoggerProvider(new FileLoggerProvider("./pilipala.log"))
        .build ()

//Thaumy
let user =
    App().UserLogin(cfg.pl_user, cfg.pl_pwd).unwrap ()
