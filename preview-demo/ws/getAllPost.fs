module ws.getAllPost

open System
open app.user
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

        let arr =
            user.GetReadablePost().foldl
            <| fun acc (x: Post) ->
                $"""{{
                    "Id": {x.Id},
                    "Title":"{x.Title.unwrap ()}",
                    "Body":{x.Body.unwrap().serializeToJson().json},
                    "CreateTime":"{x.CreateTime.unwrap ()}",
                    "ModifyTime":"{x.ModifyTime.unwrap ()}",
                    "CoverUrl":null,
                    "Summary":null,
                    "ViewCount":12384,
                    "Comments":[],
                    "CanComment":{x.CanComment.ToString().ToLower()},
                    "IsArchive":false,
                    "IsSchedule":false,
                    "Topics":[]
                }}"""
                :: acc
            <| []

        let json = arr.serializeToJson().json
        Console.WriteLine(json)
        self.Send(json)
