namespace ws.api.comment.create

open System
open fsharper.op
open fsharper.typ
open fsharper.alias

open pilipala.container.comment

type Rsp =
    { Id: i64
      UserName: string
      UserSiteUrl: string
      UserAvatarUrl: string
      Body: string
      Binding: i64
      IsReply: bool
      CreateTime: DateTime }

    static member fromComment(comment: Comment, binding: i64, isReply: bool) =
        { Id = comment.Id
          UserName = comment.UserName.unwrapOr (fun _ -> comment.UserId.ToString())
          Body = comment.Body.unwrap ()
          Binding = binding
          IsReply = isReply
          UserSiteUrl =
            comment.["UserSiteUrl"]
                .unwrap()
                .unwrap()
                .cast<Option'<string>>()
                .unwrapOr (fun _ -> null)
          UserAvatarUrl =
            comment.["UserAvatarUrl"]
                .unwrap()
                .unwrap()
                .cast<Option'<string>>()
                .unwrapOr (fun _ -> null)
          CreateTime = comment.CreateTime.unwrap () }
