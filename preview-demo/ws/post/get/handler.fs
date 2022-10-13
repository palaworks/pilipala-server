namespace ws.api.post.get

open app.user
open fsharper.op
open fsharper.typ
open pilipala.container.post
open pilipala.container.comment
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            user.GetPost(req.Id).fmap Rsp.fromPost
