namespace app.cfg

open pilipala.util.io
open pilipala.util.text
open app.cfg.plugin
open app.cfg.database

type Cfg() =
    member val plugin = PluginCfg() with get, set
    member val database = DatabaseCfg() with get, set
    member val logging = LoggingCfg() with get, set
    
    static member readFrom path =
        { toml = readFile path }
            .deserializeTo<Cfg>()
            .unwrap ()
