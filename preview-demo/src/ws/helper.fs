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

                    let api_req =
                        Some(JsonConvert.DeserializeObject<ApiRequest<_>> e.Data)
                    //{ json = e.Data }.deserializeTo<ApiRequest<_>> ()

                    let result =
                        match api_req with
                        | Some x -> self.handle x.Data
                        | None -> Err "Invalid api request"

                    let api_rsp =
                        match result with
                        | Ok x ->
                            { Seq = api_req.unwrap().Seq
                              Ok = true
                              Msg = "Success"
                              Data = x }
                        | Err msg ->
                            { Seq = -1
                              Ok = false
                              Msg = msg
                              Data = Unchecked.defaultof<'rsp> }

                    $"send {typeof<'h>.FullName} rsp:\n{api_rsp.serializeToJson().json}"
                    |> Console.WriteLine

                    b.Send(api_rsp.serializeToJson().json) }
