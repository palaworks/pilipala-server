namespace ws.api.post.get_batch

open app.user
open fsharper.typ
open fsharper.op.Foldable
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            let arr =
                req.PostIds.foldr
                <| fun id acc ->
                    match user.GetPost(id) with
                    | Ok post -> ws.api.post.get.Rsp.fromPost post :: acc
                    | _ -> acc
                <| []
                |> List.toArray

            { Collection = arr } |> Ok
