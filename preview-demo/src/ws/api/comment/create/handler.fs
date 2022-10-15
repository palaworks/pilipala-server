namespace ws.api.comment.create

open app.user
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with
        override self.handle req =

            let anonymous =
                App()
                    .UserLogin("Anonymous", "anonymous12384")
                    .unwrap ()

            if req.IsReply then
                anonymous.GetComment(req.Binding).bind
                <| fun comment ->
                    comment.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, req.Binding, true)
            else
                anonymous.GetPost(req.Binding).bind
                <| fun post ->
                    post.NewComment(req.Body).fmap
                    <| fun comment -> Rsp.fromComment (comment, req.Binding, false)
