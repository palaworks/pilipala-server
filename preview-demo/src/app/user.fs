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
        .usePlugin("./plugin/Pinned")
        .usePlugin("./plugin/Cacher")
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
