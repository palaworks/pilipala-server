module ws.server

open WebSocketSharp.Server

let wsServer =
    WebSocketServer("ws://localhost:8080")
