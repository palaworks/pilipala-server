namespace ws.helper

open System
open pilipala.util.text
open Newtonsoft.Json
open WebSocketSharp.Server
open fsharper.op
open fsharper.typ
open fsharper.alias

type EmptyReq() =
    class
    end

type ApiRequest<'req> = { Seq: i64; Data: 'req }

type ApiResponse<'rsp> =
    { Seq: i64
      Ok: bool
      Msg: string
      Data: 'rsp }

type IApiHandler<'h, 'req, 'rsp> =
    abstract handle: 'req -> Result'<'rsp, string>

module ext =
    type IApiHandler<'h, 'req, 'rsp> with
        member self.toWsBehavior() =
            { new WebSocketBehavior() with
                override b.OnMessage e =
                    $"recv {typeof<'h>.FullName} req:\n{e.Data}"
                    |> Console.WriteLine

                    let opt_api_req =
                        { json = e.Data }.deserializeTo<ApiRequest<_>> ()
                    //Some(JsonConvert.DeserializeObject<ApiRequest<_>> e.Data)

                    let result =
                        match opt_api_req with
                        | Some api_req -> self.handle api_req.Data
                        | None -> Err "Invalid api request"

                    let api_rsp =
                        match result with
                        | Ok x ->
                            { Seq = opt_api_req.unwrap().Seq
                              Ok = true
                              Msg = "Success"
                              Data = x }
                        | Err msg ->
                            { Seq =
                                //TODO 不优雅
                                match opt_api_req with
                                | Some api_req -> api_req.Seq
                                | None -> -1
                              Ok = false
                              Msg = msg
                              Data = Unchecked.defaultof<'rsp> }

                    $"send {typeof<'h>.FullName} rsp:\n{api_rsp.serializeToJson().json}"
                    |> Console.WriteLine

                    b.Send(api_rsp.serializeToJson().json) }

    type WebSocketServer with
        member self.addService(path, f: unit -> WebSocketBehavior) =
            self.AddWebSocketService(path, f)
            self
