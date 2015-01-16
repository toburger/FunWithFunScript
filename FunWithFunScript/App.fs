[<FunScript.JS>]
module App

open Angular
open FunScript.TypeScript

let appController =
    Ng.controller "appController" ["$scope"] <| fun (scope: ng.IScope) ->
        scope?hello <- "hello"
        scope?setHello <- fun () -> scope?hello <- "new value"
        scope?getHello <- fun () -> Globals.alert(scope?hello)

let app () =
    Ng.module' "app" [||]
    |> appController
