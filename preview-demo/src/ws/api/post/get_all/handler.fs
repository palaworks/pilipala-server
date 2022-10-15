namespace ws.api.post.get_all

open Microsoft.FSharp.Core
open app.user
open fsharper.op
open fsharper.typ
open ws.api.post.get
open ws.helper

type Handler() =
    interface IApiHandler<Handler, Req, ws.api.post.get_all.Rsp> with

        override self.handle req =

            let arr =
                user
                    .GetReadablePost()
                    .map(Rsp.fromPost)
                    .toArray ()

            { Collection = arr } |> Ok
