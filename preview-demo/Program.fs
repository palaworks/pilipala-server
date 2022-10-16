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

wsLocalServer.AddWebSocketService("/post/get", post.get.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/post/get_all", post.get_all.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/post/get_prev", post.get_prev.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/post/get_next", post.get_next.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/post/get_batch", post.get_batch.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/post/get_all_id", post.get_all_id.Handler().toWsBehavior)
wsLocalServer.AddWebSocketService("/comment/create", comment.create.Handler().toWsBehavior)

wsPublicServer.AddWebSocketService("/post/get", post.get.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/post/get_all", post.get_all.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/post/get_prev", post.get_prev.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/post/get_next", post.get_next.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/post/get_batch", post.get_batch.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/post/get_all_id", post.get_all_id.Handler().toWsBehavior)
wsPublicServer.AddWebSocketService("/comment/create", comment.create.Handler().toWsBehavior)

type Worker() =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        fun _ ->
            wsLocalServer.Start()
            wsPublicServer.Start()
        |> Task.RunAsTask

Host
    .CreateDefaultBuilder()
    .ConfigureServices(fun ctx services -> services.AddHostedService<Worker>() |> ignore)
    .Build()
    .Run()
