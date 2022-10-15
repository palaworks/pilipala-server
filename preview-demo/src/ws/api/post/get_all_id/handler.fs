namespace ws.api.post.get_all_id

open Microsoft.FSharp.Core
open app.user
open fsharper.op
open fsharper.typ
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, Rsp> with

        override self.handle req =

            let arr =
                user
                    .GetReadablePost()
                    .map(fun post -> post.Id)
                    .toArray ()

            { PostIds = arr } |> Ok
