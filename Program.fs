open System
open WebSocketSharp.Server
open fsharper.typ
open fsharper.op.Async
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open System.Threading.Tasks
open System
open pilipala.builder
open pilipala.plugin
open NReco.Logging.File
open fsharper.op
open fsharper.op.Foldable
open fsharper.typ
open app.cfg

type Worker() =
    inherit BackgroundService()

    override self.ExecuteAsync _ =

        Builder
            .make()
            .useDb(getDbCfg ())
            .apply(cfg.plugins.foldl <| fun b -> b.usePlugin)
            .useLoggerProvider(new FileLoggerProvider("./log/pilipala.log"))
            .build ()
        |> ignore

        Task.CompletedTask

Host
    .CreateDefaultBuilder()
    .ConfigureServices(fun ctx services -> services.AddHostedService<Worker>() |> ignore)
    .Build()
    .Run()
