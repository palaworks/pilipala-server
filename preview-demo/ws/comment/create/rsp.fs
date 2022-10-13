namespace ws.api.comment.create

open System
open fsharper.op
open fsharper.typ
open fsharper.alias

open pilipala.container.comment

type Rsp =
    { Id: i64
      User: string
      Body: string
      Binding: i64
      IsReply: bool
      SiteUrl: string
      AvatarUrl: string
      CreateTime: DateTime }

    static member fromComment(comment: Comment, binding: i64, isReply: bool) =
        { Id = comment.Id
          User = comment.UserName.unwrapOr (fun _ -> comment.UserId.ToString())
          Body = comment.Body.unwrap ()
          Binding = binding
          IsReply = isReply
          SiteUrl =
            comment.["UserSiteUrl"]
                .unwrap()
                .unwrap()
                .cast<Option'<string>>()
                .unwrapOr (fun _ -> null)
          AvatarUrl =
            comment.["UserAvatarUrl"]
                .unwrap()
                .unwrap()
                .cast<Option'<string>>()
                .unwrapOr (fun _ -> null)
          CreateTime = comment.CreateTime.unwrap () }
