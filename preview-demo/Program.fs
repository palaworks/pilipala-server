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

wsLocalServer
    .addService("/post/get", post.get.Handler().toWsBehavior)
    .addService("/post/get_all", post.get_all.Handler().toWsBehavior)
    .addService("/post/get_prev", post.get_prev.Handler().toWsBehavior)
    .addService("/post/get_next", post.get_next.Handler().toWsBehavior)
    .addService("/post/get_batch", post.get_batch.Handler().toWsBehavior)
    .addService("/post/get_all_id", post.get_all_id.Handler().toWsBehavior)
    .addService ("/comment/create", comment.create.Handler().toWsBehavior)
|> ignore

wsPublicServer
    .addService("/post/get", post.get.Handler().toWsBehavior)
    .addService("/post/get_all", post.get_all.Handler().toWsBehavior)
    .addService("/post/get_prev", post.get_prev.Handler().toWsBehavior)
    .addService("/post/get_next", post.get_next.Handler().toWsBehavior)
    .addService("/post/get_batch", post.get_batch.Handler().toWsBehavior)
    .addService("/post/get_all_id", post.get_all_id.Handler().toWsBehavior)
    .addService ("/comment/create", comment.create.Handler().toWsBehavior)
|> ignore

type Worker() =
    inherit BackgroundService()

    override self.ExecuteAsync _ =
        fun _ ->
            wsLocalServer.Start()
            wsPublicServer.Start()
        |> Task.RunAsTask

(*
    "./plugin/Markdown",
    "./plugin/PostCover",
    "./plugin/PostStatus",
    "./plugin/Topics",
    "./plugin/ViewCount",
    "./plugin/Summarizer",
    "./plugin/PartialOrder",
    "./plugin/UserName",
    "./plugin/UserAvatarUrl",
    "./plugin/UserSiteUrl",
    "./plugin/Pinned",
    "./plugin/Cacher"
*)
Host
    .CreateDefaultBuilder()
    .ConfigureServices(fun ctx services -> services.AddHostedService<Worker>() |> ignore)
    .Build()
    .Run()
