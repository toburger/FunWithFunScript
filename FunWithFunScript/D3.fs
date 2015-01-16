[<FunScript.JS>]
module D3

open FunScript.TypeScript

let max (f: _ -> float) array = Globals.d3.max(array, System.Func<_,  float>(f))
let min (f: _ -> float) array = Globals.d3.min(array, System.Func<_,  float>(f))
let map f (array: _ array) = array.map(System.Func<_,_,_,_>(f))
let inline translate x y = sprintf "translate(%i,%i)" (int x) (int y)
let inline rotate angle = sprintf "rotate(%i)" (int angle)

module d3 =
    let select name = Globals.d3.select.Invoke(name: string)
    let linearScale () = Globals.d3.scale.linear()
    let ordinalScale () = Globals.d3.scale.ordinal()
    let tsv url onSuccess onError =
        Globals.d3.tsv.Invoke(url, System.Func<_,_,_>(fun error data ->
            let error = unbox<XMLHttpRequest> error
            if unbox<string> error <> null              // hacky
            then onError (exn error.responseText)       // custom exception type would be nice
            else onSuccess (data |> Array.map unbox)))
    let asyncTsv url: Async<_> =
        Async.FromContinuations <| fun (success, error, _) ->
            tsv url success error |> ignore

[<AutoOpen>]
module Selection =
    let select name (selection: D3.Selection) = selection.select.Invoke(name: string)
    let selectAll name (selection: D3.Selection) = selection.selectAll.Invoke(name: string)
    let data array (selection: D3.Selection) = selection.data.Invoke(array |> Array.map box)
    let enter (selection: D3.UpdateSelection) = selection.enter.Invoke() |> unbox<D3.Selection>
    let append name (selection: D3.Selection) = selection.append.Invoke(name: string)
    let attr name f (selection: D3.Selection) = selection.attr.Invoke(name, System.Func<'a,float,'b>(f))
    let attrInt name (f: _ -> _ -> int) selection = attr name f selection
    let attrVal name value (selection: D3.Selection) = selection.attr.Invoke((name: string), (value: obj))
    let style name value (selection: D3.Selection) = selection.style.Invoke(name, string value)
    let text f (selection: D3.Selection) = selection.text.Invoke(System.Func<'a,float,string>(f))
    let textVal value (selection: D3.Selection) = selection.text.Invoke(string value)
    let call f (selection: D3.Selection) = selection.call(System.Func<_, _>(f))

[<AutoOpen>]
module Scale =
    let domain array (scale: D3.Scale.Scale) = downcast scale.domain.Invoke(array |> Array.map box)
    let range (from, to') (scale: D3.Scale.Scale) = downcast scale.range.Invoke([| from; to' |] |> Array.map box)
    let rangeBands (from, to') (scale: D3.Scale.OrdinalScale) = scale.rangeBands([| from; to' |] |> Array.map box)
    let rangeRoundBands (from, to') padding (scale: D3.Scale.OrdinalScale) = scale.rangeRoundBands([| from; to' |] |> Array.map box, padding)

[<AutoOpen>]
module Axis =
    let axis () = Globals.d3.svg.axis()
    let scale (scale: D3.Scale.Scale) (axis: D3.Svg.Axis) = axis.scale.Invoke(scale)
    let orient orientation (axis: D3.Svg.Axis) = axis.orient.Invoke(orientation)
    let ticks (ticks, (formatting: string)) (axis: D3.Svg.Axis) = axis.ticks.InvokeOverload2([| box ticks; box formatting |])

type Margin = { top: int; right: int; bottom: int; left: int }

type D3.Scale.Scale with
    member self.Compute x =
        downcast self.Invoke(x)
