namespace ws.api.post.get_next

open app.user
open fsharper.op
open fsharper.typ
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            user.GetPost(req.CurrentId).bind
            <| fun post ->
                post.["SuccId"].bind
                <| fun optId ->
                    match optId with
                    | None -> Err "No post available"
                    | Some id -> id |> cast |> user.GetPost |> fmap Rsp.fromPost
