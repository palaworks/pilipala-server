module ws.post.getNextPost

open System
open app.user
open fsharper.op
open fsharper.typ
open fsharper.alias
open fsharper.op.Foldable
open pilipala.container.comment
open pilipala.util.text
open WebSocketSharp.Server
open getPost


type getNextPost() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getNextPost from client:{e.Data}"

        //TODO err handler

        let prev_post =
            user.GetPost(Int64.Parse e.Data) |> bind
            <| fun post ->
                post.["SuccId"].fmap
                <| fun exist ->
                    exist |> bind <| cast
                    |> fmap cast
                    |> fmap (
                        user.GetPost
                        .> fmap Some
                        .> (fun k -> unwrapOr k (always None))
                    )
                    |> unwrapOr
                    <| (always None)
            |> fmap Some
            |> unwrapOr
            <| always None
            |> bind
            <| id

        let json =
            prev_post
            |> fmap (fun x -> x.encodeToJson ())
            |> unwrapOr
            <| always ""

        self.Send(json)
