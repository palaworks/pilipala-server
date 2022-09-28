module ws.server

open System
open System.Security.Cryptography.X509Certificates
open WebSocketSharp.Server
open app
open cfg
open pilipala.util.io

let wsServer =
    if cfg.enable_wss then
        let x = WebSocketServer(8080, true)

        let pem_path = cfg.cert_pem_path
        let key_path = cfg.cert_key_path

        let X509cert =
            X509Certificate2.CreateFromPemFile(pem_path, key_path)

        x.SslConfiguration.ServerCertificate <- X509cert
        x
    else
        WebSocketServer("ws://localhost:8080")
