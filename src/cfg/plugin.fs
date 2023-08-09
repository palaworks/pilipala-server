namespace app.cfg.plugin

type PluginCfg() =
    member val paths: string array = [||] with get, set
