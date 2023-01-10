namespace app.host

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open app.host.worker

[<AutoOpen>]
module ext_IHostBuilder =
    type IHostBuilder with
        member self.AddHostedService f =
            fun _ (sc: IServiceCollection) ->
                fun _ -> BackgroundService.from f
                |> sc.AddHostedService
                |> ignore
            |> self.ConfigureServices
