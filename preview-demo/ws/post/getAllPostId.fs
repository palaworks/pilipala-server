module ws.post.getAllPostId

open System
open app.user
open pilipala.container.comment
open pilipala.util.text
open pilipala.container.post
open fsharper.op
open fsharper.typ
open fsharper.op.Foldable
open WebSocketSharp.Server

type getAllPostId() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getAllPostId from client:{e.Data}"

        let json =
            user.GetReadablePost().foldr
            <| fun (post: Post) acc -> $",{post.Id}{acc}"
            <| "]"
            |> fun s -> $"[{s.Substring(1)}"

        self.Send(json)
