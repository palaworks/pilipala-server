open System
open ws.server
open ws.getPost
open ws.getAllPost
open ws.getAllPostId
open ws.getPrevPost
open ws.getNextPost

wsServer.AddWebSocketService<getPost>("/get_post")
wsServer.AddWebSocketService<getAllPost>("/get_all_post")
wsServer.AddWebSocketService<getAllPostId>("/get_all_post_id")
wsServer.AddWebSocketService<getPrevPost>("/get_prev_post")
wsServer.AddWebSocketService<getNextPost>("/get_next_post")

wsServer.Start()
Console.ReadKey() |> ignore
wsServer.Stop()
