module ws.getPost

open System
open app.user
open fsharper.op
open fsharper.typ
open fsharper.alias
open fsharper.op.Foldable
open pilipala.container.post
open pilipala.container.comment
open pilipala.util.text
open WebSocketSharp.Server

type CommentJson =
    { Id: i64
      User: string
      Body: string
      ReplyTo: i64
      SiteUrl: string
      AvatarUrl: string
      CreateTime: string }

type PostJson =
    { Id: i64
      Title: string
      Body: string
      CreateTime: string
      ModifyTime: string
      CoverUrl: string
      Summary: string
      IsGeneratedSummary: bool
      ViewCount: u32
      Comments: CommentJson array
      CanComment: bool
      IsArchived: bool
      IsScheduled: bool
      Topics: string array
      PrevId: i64
      NextId: i64 }

type Post with
    member post.encodeToJson() =

        let rec getRecursiveComments (comment: Comment) =
            match comment.Comments.unwrap().toList () with
            | [] -> []
            | cs ->
                cs.foldl
                <| fun acc c -> acc @ (comment.Id, c) :: getRecursiveComments c
                <| []

        let comments =
            (post.Comments.unwrap().foldl
             <| fun acc c -> acc @ (post.Id, c) :: getRecursiveComments c
             <| []
             |> List.sortBy (fun x -> x.snd().CreateTime |> unwrap))
                .foldr
            <| fun (replyTo, comment: Comment) acc ->
                { Id = comment.Id
                  User = comment.UserName.unwrapOr (fun _ -> comment.UserId.ToString())
                  Body = comment.Body.unwrap ()
                  ReplyTo = replyTo
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
                :: acc
            <| []

        { Id = post.Id
          Title = post.Title.unwrap ()
          Body = post.Body.unwrap ()
          CreateTime = post.CreateTime.unwrap().ToIso8601()
          ModifyTime = post.ModifyTime.unwrap().ToIso8601()
          CoverUrl =
            post.["CoverUrl"]
                .unwrap()
                .unwrap()
                .cast<Option'<string>>()
                .unwrapOr (fun _ -> null)
          Summary =
            post.["Summary"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> null)
          IsGeneratedSummary =
            post.["IsGeneratedSummary"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> false)
          ViewCount =
            post.["ViewCount"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> 0u)
          Comments = comments.toArray ()
          CanComment = post.CanComment
          IsArchived =
            post.["IsArchived"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> false)
          IsScheduled =
            post.["IsScheduled"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> false)
          Topics =
            post.["Topics"]
                .unwrap()
                .fmap(cast)
                .unwrapOr (fun _ -> [||])
          PrevId =
            post.["PredId"]
                .unwrap()
                .unwrap()
                .cast<Option'<obj>>()
                .fmap(fun id -> id.cast<i64> ())
                .unwrapOr (fun _ -> -1)
          NextId =
            post.["SuccId"]
                .unwrap()
                .unwrap()
                .cast<Option'<obj>>()
                .fmap(fun id -> id.cast<i64> ())
                .unwrapOr (fun _ -> -1) }
            .serializeToJson()
            .json


type getPost() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getPost from client:{e.Data}"

        let post =
            user.GetPost(Int64.Parse e.Data).unwrap ()

        let json = post.encodeToJson ()

        self.Send(json)
