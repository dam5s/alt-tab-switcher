module SwitcherLib.MainWindowHelper

open System.Windows
open System.Windows.Controls
open System.Windows.Media
open SwitcherLib.AppProcesses

let private cellWidth = 200.0
let private cellHeight = 200.0
let private padding = 10.0

let private newColumn _ =
    let c = ColumnDefinition()
    c.Width <- GridLength(cellWidth)
    c

let private newRow _ =
    let c = RowDefinition()
    c.Height <- GridLength(cellHeight)
    c

let private addColumn (grid: Grid) _ =
    grid.ColumnDefinitions.Add (newColumn ())

let private addProcess (grid: Grid) (index: int) (app: AppProcess) =
    let text = TextBlock()
    text.Text <- app.Title
    text.Width <- cellWidth
    text.Padding <- Thickness(padding, 0.0, padding, 0.0)
    text.FontFamily <- FontFamily("Trebuchet MS")
    text.FontSize <- 14.0
    text.TextAlignment <- TextAlignment.Center
    text.FontWeight <- FontWeight.FromOpenTypeWeight(600)
    text.Foreground <- SolidColorBrush(Color.FromScRgb(1.0f, 1.0f, 1.0f, 1.0f))
    text.VerticalAlignment <- VerticalAlignment.Center

    Grid.SetColumn (text, index)

    grid.Children.Add text |> ignore

let configure (window: Window, grid: Grid) =
    let p = AppProcesses.get ()

    window.Title <- "Alt-Tab Switcher"
    window.WindowStyle <- WindowStyle.None
    window.ShowInTaskbar <- true
    window.AllowsTransparency <- true
    window.Background <- SolidColorBrush(Color.FromScRgb(0.8f, 0.0f, 0.0f, 0.0f))
    window.Width <- (cellWidth * float p.Length) + padding * 2.0
    window.Height <- cellHeight + padding * 2.0
    window.WindowStartupLocation <- WindowStartupLocation.CenterScreen

    grid.RowDefinitions.Add (newRow ())
    grid.Margin <- Thickness(padding)

    p |> Seq.iter (addColumn grid)
    p |> Seq.iteri (addProcess grid)

    ()
