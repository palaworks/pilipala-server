module ws.server

open System.Net
open System.Security.Cryptography.X509Certificates
open WebSocketSharp.Server
open app
open cfg

let wsLocalServer =
    WebSocketServer(IPAddress.Any, cfg.ws_local_port, false)

let wsPublicServer =
    let x =
        WebSocketServer(IPAddress.Any, cfg.ws_public_port, true)

    let pem_path = cfg.ws_cert_pem_path
    let key_path = cfg.ws_cert_key_path

    let X509cert =
        X509Certificate2.CreateFromPemFile(pem_path, key_path)

    x.SslConfiguration.ServerCertificate <- X509cert
    x
