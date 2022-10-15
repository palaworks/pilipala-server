namespace ws.api.post.get

open ws
open fsharper.alias
open fsharper.op
open fsharper.typ
open fsharper.op.Foldable
open pilipala.container.post
open pilipala.container.comment
open pilipala.util.text
open ws.api.comment.create

type Rsp =
    { Id: i64
      Title: string
      Body: string
      CreateTime: string
      ModifyTime: string
      CoverUrl: string
      Summary: string
      IsGeneratedSummary: bool
      ViewCount: u32
      Comments: api.comment.create.Rsp array
      CanComment: bool
      IsArchived: bool
      IsScheduled: bool
      Topics: string array
      PrevId: i64
      NextId: i64 }

    static member fromPost(post: Post) =
        let rec getRecursiveComments (comment: Comment) =
            match comment.Comments.unwrap().toList () with
            | [] -> []
            | cs ->
                cs.foldl
                <| fun acc c ->
                    acc
                    @ (comment.Id, true, c) :: getRecursiveComments c
                <| []

        let comments =
            (post.Comments.unwrap().foldl
             <| fun acc c ->
                 acc
                 @ (post.Id, false, c) :: getRecursiveComments c
             <| []
             |> List.sortBy (fun (_, _, c) -> c.CreateTime |> unwrap))
                .foldr
            <| fun (replyTo, isReply, comment: Comment) acc ->
                let user =
                    comment.["UserName"]
                        .unwrap() //Opt<obj>
                        .fmap(cast) //Opt<str>
                        .unwrapOr (fun _ -> comment.UserId.ToString())

                let siteUrl =
                    comment.["UserSiteUrl"]
                        .unwrap() //Opt<Opt<obj>>
                        .fmap(fun x -> x.cast<Option'<string>> ())
                        .bind(id)
                        .unwrapOr (fun _ -> null)

                let avatarUrl =
                    comment.["UserAvatarUrl"]
                        .unwrap() //Opt<Opt<obj>>
                        .fmap(fun x -> x.cast<Option'<string>> ())
                        .bind(id)
                        .unwrapOr (fun _ -> null)

                Rsp.fromComment (comment, comment.Id, isReply)
                :: acc
            <| []

        let coverUrl =
            post.["CoverUrl"]
                .unwrap() //Opt<Opt<obj>>
                .fmap(cast) //Opt<Opt<str>>
                .bind(id) //Opt<str>
                .unwrapOr (fun _ -> null)

        let summary =
            post.["Summary"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<str>
                .unwrapOr (fun _ -> null)

        let isGeneratedSummary =
            post.["IsGeneratedSummary"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<bool>
                .unwrapOr (fun _ -> false)

        let viewCount =
            post.["ViewCount"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<u32>
                .unwrapOr (fun _ -> 0u)

        let isArchived =
            post.["IsArchived"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<bool>
                .unwrapOr (fun _ -> false)

        let isScheduled =
            post.["IsScheduled"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<bool>
                .unwrapOr (fun _ -> false)

        let topics =
            post.["Topics"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<[str]>
                .unwrapOr (fun _ -> [||])

        let predId =
            post.["PredId"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<Opt<obj>>
                .bind(id) //Opt<obj>
                .fmap(fun id -> id.cast<i64> ()) //Opt<i64>
                .unwrapOr (fun _ -> -1)

        let succId =
            post.["SuccId"]
                .unwrap() //Opt<obj>
                .fmap(cast) //Opt<Opt<obj>>
                .bind(id) //Opt<obj>
                .fmap(fun id -> id.cast<i64> ()) //Opt<i64>
                .unwrapOr (fun _ -> -1)

        { Id = post.Id
          Title = post.Title.unwrap ()
          Body = post.Body.unwrap ()
          CreateTime = post.CreateTime.unwrap().ToIso8601()
          ModifyTime = post.ModifyTime.unwrap().ToIso8601()
          CoverUrl = coverUrl
          Summary = summary
          IsGeneratedSummary = isGeneratedSummary
          ViewCount = viewCount
          Comments = comments.toArray ()
          CanComment = post.CanComment
          IsArchived = isArchived
          IsScheduled = isScheduled
          Topics = topics
          PrevId = predId
          NextId = succId }
