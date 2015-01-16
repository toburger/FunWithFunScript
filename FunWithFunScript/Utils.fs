[<AutoOpen>]
module Utils

open System.Reflection
open System.IO
open Suave.Http
open Suave.Http.Writers
open Suave.Http.Successful
open FunScript

let read_resource name =
    let assm = Assembly.GetExecutingAssembly()
    use strm = assm.GetManifestResourceStream(name)
    use rdr = new StreamReader(strm)
    rdr.ReadToEnd()

let compile_js expression =
    #if DEBUG
    Compiler.compileWithoutReturn expression
    #else
    Compiler.Compiler.Compile(expression = expression,
                              noReturn = true,
                              shouldCompress = true)
    #endif

let OK_js s =
    set_mime_type "application/javascript"
    >>= OK s

let mime_types_map =
    default_mime_types_map
    >=> function
        | ".tsv" -> mk_mime_type "application/tsv" true
        | ".csv" -> mk_mime_type "application/csv" true
        | _ -> None
