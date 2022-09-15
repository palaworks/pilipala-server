module ws.getPost

open System
open app.user
open pilipala.util.text
open WebSocketSharp.Server

type getPost() =
    inherit WebSocketBehavior()

    override self.OnMessage e =
        Console.WriteLine $"getPost from client:{e.Data}"

        let x =
            user.GetPost(Int64.Parse e.Data).unwrap ()

        let json =
            $"""{{
                "Id": {x.Id},
                "Title":"{x.Title.unwrap ()}",
                "Body":{x.Body.unwrap().serializeToJson().json},
                "CreateTime":"{x.CreateTime.unwrap ()}",
                "ModifyTime":"{x.ModifyTime.unwrap ()}",
                "CoverUrl":{x.["CoverUrl"].unwrap().unwrapOr (fun _ -> "null")},
                "Summary":{x.["CoverUrl"].unwrap().unwrapOr (fun _ -> "null")},
                "ViewCount":{x.["CoverUrl"].unwrap().unwrapOr (fun _ -> "0")},
                "Comments":[],
                "CanComment":{x.CanComment.ToString().ToLower()},
                "IsArchive":{x.["CoverUrl"]
                                 .unwrap()
                                 .fmap(fun x -> x.ToString().ToLower())
                                 .unwrapOr (fun _ -> "false")},
                "IsSchedule":{x.["CoverUrl"]
                                  .unwrap()
                                  .fmap(fun x -> x.ToString().ToLower())
                                  .unwrapOr (fun _ -> "false")},
                "Topics":{x.["CoverUrl"]
                              .unwrap()
                              .fmap(fun x -> x.ToString().ToLower())
                              .unwrapOr (fun _ -> "false")}
            }}"""

        Console.WriteLine(json)
        self.Send(json)
