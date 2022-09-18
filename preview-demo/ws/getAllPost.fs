module ws.getAllPost

open System
open app.user
open pilipala.container.comment
open pilipala.util.text
open pilipala.container.post
open fsharper.op
open fsharper.typ
open fsharper.op.Foldable
open WebSocketSharp.Server

type getAllPost() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getAllPost from client:{e.Data}"

        let json =
            user.GetReadablePost().foldr
            <| fun (post: Post) acc ->

                let rec getRecursiveComments (comment: Comment) =
                    match comment.Comments.unwrap().toList () with
                    | [] -> []
                    | cs ->
                        cs.foldl
                        <| fun acc c -> acc @ (comment.Id, c) :: getRecursiveComments c
                        <| []

                let comments_json =
                    (post.Comments.unwrap().foldl
                     <| fun acc c -> acc @ (post.Id, c) :: getRecursiveComments c
                     <| []
                     |> List.sortBy (fun x -> x.snd().CreateTime |> unwrap))
                        .foldr
                    <| fun (replyTo, comment: Comment) acc ->
                        let json =
                            $"""{{
                                "Id":{comment.Id},
                                "User":"{comment.UserId.unwrap ()}",
                                "Body":{comment.Body.unwrap().serializeToJson().json},
                                "ReplyTo":{replyTo},
                                "SiteUrl":"https://www.thaumy.cn",
                                "AvatarUrl":"/src/assets/comment_user_avatars/kurumi.jpg",
                                "CreateTime":"{comment.CreateTime.unwrap().ToIso8601()}"
                            }}"""

                        $",{json}{acc}"
                    <| ""
                    |> fun s ->
                        $"[{if s.Length <> 0 then
                                s.Substring(1)
                            else
                                String.Empty}]"

                let json =
                    $"""{{
                        "Id": {post.Id},
                        "Title":"{post.Title.unwrap ()}",
                        "Body":{post.Body.unwrap().serializeToJson().json},
                        "CreateTime":"{post.CreateTime.unwrap().ToIso8601()}",
                        "ModifyTime":"{post.ModifyTime.unwrap().ToIso8601()}",
                        "CoverUrl":{post.["CoverUrl"]
                                        .unwrap()
                                        .unwrap()
                                        .cast<Option'<string>>()
                                        .fmap(fun s -> $"\"{s}\"")
                                        .unwrapOr (fun _ -> "null")},
                        "Summary":{post.["Summary"]
                                       .unwrap()
                                       .fmap(fun s -> s.serializeToJson().json)
                                       .unwrapOr (fun _ -> "null")},
                        "IsGeneratedSummary":{post.["IsGeneratedSummary"]
                                                  .unwrap()
                                                  .fmap(fun b -> b.ToString().ToLower())
                                                  .unwrapOr (fun _ -> "false")},
                        "ViewCount":{post.["ViewCount"]
                                         .unwrap()
                                         .unwrapOr (fun _ -> "0")},
                        "Comments":{comments_json},
                        "CanComment":{post.CanComment.ToString().ToLower()},
                        "IsArchived":{post.["IsArchived"]
                                          .unwrap()
                                          .fmap(fun b -> b.ToString().ToLower())
                                          .unwrapOr (fun _ -> "false")},
                        "IsScheduled":{post.["IsScheduled"]
                                           .unwrap()
                                           .fmap(fun b -> b.ToString().ToLower())
                                           .unwrapOr (fun _ -> "false")},
                        "Topics":{post.["Topics"].unwrap().unwrapOr(fun _ -> [||])
                                      .serializeToJson()
                                      .json}
                    }}"""

                $",{json}{acc}"
            <| "]"
            |> fun s -> $"[{s.Substring(1)}"

        self.Send(json)
