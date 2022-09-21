open System
open ws.server
open ws.getPost
open ws.getAllPost
open ws.getAllPostId

wsServer.AddWebSocketService<getPost>("/get_post")
wsServer.AddWebSocketService<getAllPost>("/get_all_post")
wsServer.AddWebSocketService<getAllPostId>("/get_all_post_id")

wsServer.Start()
Console.ReadKey() |> ignore
wsServer.Stop()
