namespace app.host.worker

open fsharper.typ
open Microsoft.Extensions.Hosting
open System.Threading.Tasks

[<AutoOpen>]
module ext_BackgroundService =

    type BackgroundService with
        static member from f =
            { new BackgroundService() with
                member self.ExecuteAsync _ =
                    f ()
                    Task.CompletedTask }
