[<FunScript.JS>]
module Angular

open FunScript
open FunScript.TypeScript

    module Ng =
        let module' name requires = Globals.angular._module(name, requires)
        let controller name (params': string list) (f: 'a -> unit) (module': ng.IModule) =
            let params' = (params' |> List.map box) @ [ box f ] |> List.toArray
            module'.controller(name, params')

        type ng.IScope with
            member self.``$watch``(watchExpression) =
                self.Dollarwatch(watchExpression: string)

let [<JSEmitInline("{0}[{1}] = {2}")>] (?<-) obj name value = ()
let [<JSEmitInline("{0}[{1}]")>] (?) (v: obj) name: 'T = unbox (obj())
