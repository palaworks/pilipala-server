open System
open ws.server
open ws.getPost
open ws.getAllPost

wsServer.AddWebSocketService<getPost>("/get_post")
wsServer.AddWebSocketService<getAllPost>("/get_all_post")

wsServer.Start()
Console.ReadKey() |> ignore
wsServer.Stop()
