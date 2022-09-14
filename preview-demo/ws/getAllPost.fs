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

        let json =
            user.GetReadablePost().foldr
            <| fun (x: Post) acc ->
                let x =
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

                $",{x}{acc}"
            <| "]"
            |> fun s -> $"[{s.Substring(1)}"

        Console.WriteLine(json)
        self.Send(json)
