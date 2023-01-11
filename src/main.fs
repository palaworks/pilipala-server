open fsharper.typ
open Microsoft.Extensions.Hosting
open pilipala.builder
open pilipala.data.db
open NReco.Logging.File
open fsharper.op
open app.cfg
open app.cfg.database
open app.host

[<EntryPoint>]
let main _ =
    let cfg =
        Cfg.readFrom "./config/config.toml"

    let f =
        Builder.make().useDb(
            DbConfig.from cfg.database
        )
            .apply(
            cfg.plugin.paths.foldl <| fun b -> b.usePlugin
        )
            .useLoggerProvider(
            new FileLoggerProvider(cfg.logging.path)
        )
            .build
        .> ignore

    Host
        .CreateDefaultBuilder()
        .AddHostedService(f)
        .Build()
        .Run()

    0
