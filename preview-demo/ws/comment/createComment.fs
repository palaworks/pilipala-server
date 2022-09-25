module ws.comment.createComment

open System
open app.user
open pilipala.container.comment
open pilipala.util.text
open pilipala.container.post
open fsharper.op
open fsharper.typ
open fsharper.op.Foldable
open ws.post.helper
open WebSocketSharp.Server

type createComment() =
    inherit WebSocketBehavior()

    member private self.send(data: string) = self.Send data

    override self.OnMessage e =
        Console.WriteLine $"sendComment from client:{e.Data}"

        let anonymous =
            App()
                .UserLogin("Anonymous", "anonymous12384")
                .unwrap ()

        let json: CommentJson =
            { json = e.Data }.deserializeTo ()

        if json.IsReply then
            anonymous
                .GetComment(json.Binding)
                .fmap(fun comment ->
                    comment
                        .NewComment(json.Body)
                        .fmap(fun comment ->
                            { Id = comment.Id
                              User = comment.UserName.unwrapOr (fun _ -> comment.UserId.ToString())
                              Body = comment.Body.unwrap ()
                              Binding = json.Binding
                              IsReply = true
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
                              CreateTime = comment.CreateTime.unwrap().ToIso8601() }
                                .serializeToJson()
                                .json)
                        .unwrapOr (fun _ -> ""))
                .unwrapOr (fun _ -> "")
            |> effect Console.WriteLine
            |> self.send
        else
            anonymous
                .GetPost(json.Binding)
                .fmap(fun post ->
                    post
                        .NewComment(json.Body)
                        .fmap(fun comment ->
                            { Id = comment.Id
                              User = comment.UserName.unwrapOr (fun _ -> comment.UserId.ToString())
                              Body = comment.Body.unwrap ()
                              Binding = json.Binding
                              IsReply = false
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
                              CreateTime = comment.CreateTime.unwrap().ToIso8601() }
                                .serializeToJson()
                                .json)
                        .unwrapOr (fun _ -> ""))
                .unwrapOr (fun _ -> "")
            |> effect Console.WriteLine
            |> self.send
