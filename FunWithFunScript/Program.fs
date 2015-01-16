open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Http.RequestErrors
open Suave.Web
open Suave.Logging

let config =
    { default_config with
        logger = Loggers.sane_defaults_for LogLevel.Debug }

let app =
    choose [
        log config.logger log_format >>= never
        url "/" >>= OK (read_resource "index.html")
        url "/app.js" >>= OK_js (compile_js <@ App.app() @>)
        NOT_FOUND "FILE NOT FOUND."
    ]

web_server config app
