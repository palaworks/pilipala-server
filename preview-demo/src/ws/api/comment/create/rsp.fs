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
        let UserName =
            comment.["UserName"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<str>
                .unwrapOr (fun _ -> comment.UserId.ToString())

        let UserSiteUrl =
            comment.["UserSiteUrl"]
                .unwrap() //Opt<Opt<obj>>
                .fmap(cast) //Opt<Opt<str>>
                .bind(id) //Opt<str>
                .unwrapOr (fun _ -> null)

        let UserAvatarUrl =
            comment.["UserAvatarUrl"]
                .unwrap() //Opt<Opt<obj>>
                .fmap(cast) //Opt<Opt<str>>
                .bind(id) //Opt<str>
                .unwrapOr (fun _ -> null)

        { Id = comment.Id
          UserName = UserName
          Body = comment.Body.unwrap ()
          Binding = binding
          IsReply = isReply
          UserSiteUrl = UserSiteUrl
          UserAvatarUrl = UserAvatarUrl
          CreateTime = comment.CreateTime.unwrap () }
