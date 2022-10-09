module ws.post.getBatchPost

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

type getBatchPost() =
    inherit WebSocketBehavior()

    member private self.send(data: string) = self.Send data

    override self.OnMessage e =
        Console.WriteLine $"getBatchPost from client:{e.Data}"

        { json = e.Data }
            .deserializeTo<List<string>>()
            .fmap(fun list ->
                list.foldl
                <| fun (acc: List<_>) post_id ->
                    user
                        .GetPost(Int64.Parse(post_id))
                        .fmap (fun post -> post.encodeToJson ())
                    |> ifCanUnwrap
                    <| acc.Add

                    acc
                <| List<_>())
            .fmap (fun list -> list.serializeToJson().json)
        |> unwrapOr
        <| (fun _ -> "")
        |> self.send
