namespace ws.helper

open System
open pilipala.util.text
open WebSocketSharp.Server
open fsharper.op
open fsharper.typ

type ApiResponse<'rsp> = { Ok: bool; Msg: string; Data: 'rsp }

type IApiHandler<'h, 'req, 'rsp> =
    abstract handle: 'req -> Result'<'rsp, string>

module ext =
    type IApiHandler<'h, 'req, 'rsp> with
        member self.toWsBehavior() =
            { new WebSocketBehavior() with
                override b.OnMessage e =
                    Console.WriteLine $"{typeof<'h>.Name} req\n:{e.Data}"

                    let req: Option'<'req> =
                        { json = e.Data }.deserializeTo().unwrap ()

                    let result =
                        match req with
                        | Some x -> self.handle x
                        | None -> Err "Invalid api arguments"

                    let rsp =
                        match result with
                        | Ok x -> { Ok = true; Msg = "Success"; Data = x }
                        | Err msg ->
                            { Ok = false
                              Msg = msg
                              Data = null.coerce () }

                    Console.WriteLine $"{typeof<'h>.Name} rsp\n:{rsp.serializeToJson().json}"

                    b.Send(rsp.serializeToJson().json) }
