open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Logging

let config =
    { default_config with
        logger = Loggers.sane_defaults_for LogLevel.Debug
        mime_types_map = mime_types_map }

let ang =
    choose [
        url "/" <|> url "/ang.html" >>= OK (read_resource "ang.html")
        url "/app.js" >>= OK_js (compile_js <@ App.Angular.render() @>)
    ]

let d3 =
    choose [
        url "/d3.html" >>= OK (read_resource "d3.html")
        url "/d3.js" >>= OK_js (compile_js <@ App.D3.render() |> Async.StartImmediate @>)
        url "/data.tsv" >>= OK (read_resource "data.tsv")
    ]

let app =
    choose [
        log config.logger log_format >>= never
        ang
        d3
        NOT_FOUND "FILE NOT FOUND."
    ]

web_server config app
