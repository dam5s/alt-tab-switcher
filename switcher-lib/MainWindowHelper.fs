module SwitcherLib.MainWindowHelper

open System.Windows
open System.Windows.Controls
open System.Windows.Media
open SwitcherLib.AppWindows

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
    grid.ColumnDefinitions.Add(newColumn ())

let private addProcess (grid: Grid) (index: int) (app: AppWindow) =
    let text = TextBlock()
    text.Text <- sprintf "%s\n%d - %s" app.Title app.ProcessId app.ProcessName
    text.Width <- cellWidth
    text.Padding <- Thickness(padding, 0.0, padding, 0.0)
    text.FontFamily <- FontFamily("Trebuchet MS")
    text.FontSize <- 14.0
    text.TextAlignment <- TextAlignment.Center
    text.FontWeight <- FontWeight.FromOpenTypeWeight(600)
    text.Foreground <- SolidColorBrush(Color.FromScRgb(1.0f, 1.0f, 1.0f, 1.0f))
    text.VerticalAlignment <- VerticalAlignment.Center

    Grid.SetColumn(text, index)

    grid.Children.Add text |> ignore

let configure (window: Window, grid: Grid) =
    let appWindows = AppWindows.get ()

    window.Title <- "Alt-Tab Switcher"
    window.WindowStyle <- WindowStyle.None
    window.ShowInTaskbar <- true
    window.AllowsTransparency <- true
    window.WindowStartupLocation <- WindowStartupLocation.CenterScreen
    window.Background <- SolidColorBrush(Color.FromScRgb(0.9f, 0.0f, 0.0f, 0.0f))
    window.Width <- (cellWidth * float appWindows.Length) + padding * 2.0
    window.Height <- cellHeight + padding * 2.0

    grid.RowDefinitions.Add(newRow ())
    grid.Margin <- Thickness(padding)

    appWindows |> Seq.iter (addColumn grid)
    appWindows |> Seq.iteri (addProcess grid)

    ()
