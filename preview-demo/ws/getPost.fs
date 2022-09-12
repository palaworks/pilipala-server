module ws.getPost

open System
open app.user
open pilipala.util.text
open WebSocketSharp.Server

type getPost() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getPost from client:{e.Data}"

        let post =
            user.GetPost(Int64.Parse e.Data).unwrap ()

        let json =
            $"""{{
                "Id": {post.Id},
                "Title":"{post.Title.unwrap ()}",
                "Body":{post.Body.unwrap().serializeToJson().json},
                "CreateTime":"{post.CreateTime.unwrap ()}",
                "ModifyTime":"{post.ModifyTime.unwrap ()}",
                "CoverUrl":null,
                "Summary":null,
                "ViewCount":12384,
                "Comments":[],
                "CanComment":{post.CanComment.ToString().ToLower()},
                "IsArchive":false,
                "IsSchedule":false,
                "Topics":[]
            }}"""

        Console.WriteLine(json)
        self.Send(json)
