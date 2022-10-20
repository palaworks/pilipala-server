namespace ws.api.post.get_next

open app.user
open fsharper.op
open fsharper.typ
open fsharper.alias
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            pl_display_user.GetPost(req.CurrentId).bind
            <| fun post ->
                post.["SuccId"].bind
                <| fun opt_opt_id ->
                    (opt_opt_id.fmap
                     <| fun x ->
                         x.cast<Option'<obj>>().fmap
                         <| fun x -> x.cast<i64> ())
                        .bind id
                    |> fun optId ->
                        match optId with
                        | None -> Err "No post available"
                        | Some id -> (pl_display_user.GetPost id) |> fmap Rsp.fromPost
