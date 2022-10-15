open System
open WebSocketSharp.Server
open fsharper.typ
open fsharper.op.Async
open ws.api
open ws.server
open ws.helper
open ws.helper.ext
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open System.Threading.Tasks

wsServer.AddWebSocketService("/post/get", post.get.Handler().toWsBehavior)
wsServer.AddWebSocketService("/post/get_all", post.get_all.Handler().toWsBehavior)
wsServer.AddWebSocketService("/post/get_prev", post.get_prev.Handler().toWsBehavior)
wsServer.AddWebSocketService("/post/get_next", post.get_next.Handler().toWsBehavior)
wsServer.AddWebSocketService("/post/get_batch", post.get_batch.Handler().toWsBehavior)
wsServer.AddWebSocketService("/post/get_all_id", post.get_all_id.Handler().toWsBehavior)
wsServer.AddWebSocketService("/comment/create", comment.create.Handler().toWsBehavior)

type Worker() =
    inherit BackgroundService()

    override self.ExecuteAsync ct =
        fun _ -> wsServer.Start()
        |> Task.RunAsTask

Host
    .CreateDefaultBuilder()
    .ConfigureServices(fun ctx services -> services.AddHostedService<Worker>() |> ignore)
    .Build()
    .Run()
