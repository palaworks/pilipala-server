module ws.post.getPost

open System
open app.user
open fsharper.op
open fsharper.typ
open fsharper.alias
open fsharper.op.Foldable
open pilipala.container.post
open pilipala.container.comment
open pilipala.util.text
open WebSocketSharp.Server
open ws.post.helper

type getPost() =
    inherit WebSocketBehavior()

    member private self.send(data: string) = self.Send data

    override self.OnMessage e =
        Console.WriteLine $"getPost from client:{e.Data}"

        user.GetPost(Int64.Parse e.Data)
        |> fmap (fun post -> post.encodeToJson().serializeToJson().json)
        |> unwrapOr
        <| (fun _ -> "")
        |> self.send
