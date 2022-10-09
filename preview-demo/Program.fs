open System
open fsharper.typ
open fsharper.op.Async
open ws.server
open ws.post.getPost
open ws.post.getBatchPost
open ws.post.getAllPostId
open ws.post.getPrevPost
open ws.post.getNextPost
open ws.comment.createComment
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open System.Threading.Tasks

wsServer.AddWebSocketService<getPost>("/get_post")
wsServer.AddWebSocketService<getBatchPost>("/get_batch_post")
wsServer.AddWebSocketService<getAllPostId>("/get_all_post_id")
wsServer.AddWebSocketService<getPrevPost>("/get_prev_post")
wsServer.AddWebSocketService<getNextPost>("/get_next_post")
wsServer.AddWebSocketService<createComment>("/create_comment")

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
