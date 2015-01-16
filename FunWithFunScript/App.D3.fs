[<FunScript.JS>]
module App.D3

open D3
open FunScript.TypeScript

type Data = { letter: string; frequency: string }

let addXAxis x chart =
    let xAxis =
        axis ()
        |> scale x
        |> orient "bottom"

    chart |> append "g"
            |> attrVal "class" "x axis"
            |> call (xAxis.Invoke)

let addYAxis y chart =
    let yAxis =
        axis ()
        |> scale y
        |> orient "left"
        |> ticks (10, "%")

    chart |> append "g"
            |> attrVal "class" "y axis"
            |> call (yAxis.Invoke)

let render () = async {

    let margin = { top = 20; right = 30; bottom = 30; left = 40 }
    let width  = 960 - margin.left - margin.right
    let height = 500 - margin.top - margin.bottom

    let chart =
        d3.select "#chart"
        |> attrVal "width" (width + margin.left + margin.right)
        |> attrVal "height" (height + margin.top + margin.bottom)
        |> append "g"
            |> attrVal "transform" (translate margin.left margin.top)

    try
        let! data = d3.asyncTsv "/data.tsv"

        let y =
            d3.linearScale ()
            |> domain [| 0.;  data |> max (fun d -> Globals.parseFloat d.frequency) |]
            |> range (height, 0)

        let x =
            d3.ordinalScale ()
            |> domain (data |> Array.map (fun d -> d.letter))
            |> rangeRoundBands (0, width) 0.1

        do
            chart |> addXAxis x
                    |> attrVal "transform" (translate 0 height)
                    |> ignore

            chart |> addYAxis y
                    |> ignore

        let bar =
            chart |> selectAll ".bar"
                    |> Selection.data data
                    |> enter
                    |> append "rect"
                    |> attrVal "class" "bar"
                    |> attrInt "x" (fun d _ -> x.Compute d.letter)
                    |> attrVal "width" (x.rangeBand())
                    |> attrInt "y" (fun d _ -> y.Compute d.frequency)
                    |> attrInt "height" (fun d _ -> int height - (y.Compute d.frequency))

        let text =
            chart |> append "text"
                    |> attrVal "transform" (rotate -90)
                    |> attrVal "y" 6
                    |> attrVal "dy" ".71em"
                    |> style "text-anchor" "end"
                    |> textVal "Frequency"

        do ()
    with ex -> Globals.console.debug (ex.Message)
}
