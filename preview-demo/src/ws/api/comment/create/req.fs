namespace ws.api.comment.create

open fsharper.alias

type Req =
    { Binding: i64
      IsReply: bool
      Body: string }
