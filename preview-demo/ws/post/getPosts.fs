module ws.post.getPosts

open System
open System.Collections.Generic
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
        Console.WriteLine $"getPosts from client:{e.Data}"

        { json = e.Data }
            .deserializeTo<List<i64>>()
            .fmap(fun list ->
                list.foldl
                <| fun (acc: List<_>) post_id ->
                    user.GetPost(post_id) |> ifCanUnwrap <| acc.Add
                    acc
                <| List<Post>())
            .fmap (fun list -> list.serializeToJson().json)
        |> unwrapOr
        <| (fun _ -> "")
        |> self.send
