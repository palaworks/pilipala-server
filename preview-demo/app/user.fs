module app.user

open pilipala.builder
open pilipala.plugin
open NReco.Logging.File
open app.cfg

let App () =
    Builder
        .make()
        .useDb(getDbCfg ())
        .usePlugin<pilipala.plugin.Markdown>()
        .usePlugin<Cacher>()
        .useLoggerProvider(new FileLoggerProvider("./pilipala.log"))
        .build ()

//Thaumy
let user =
    App().UserLogin("Thaumy", "thaumy12384").unwrap ()
