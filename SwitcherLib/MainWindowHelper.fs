module SwitcherLib.MainWindowHelper

open System.Windows
open System.Windows.Controls
open System.Windows.Media
open SwitcherLib

let private cellWidth = 300.0
let private cellHeight = 200.0
let private padding = 10.0

type IAppWindowViewFactory =
    abstract Create: AppWindowViewModel -> UIElement

let configure (window: Window, panel: StackPanel, viewFactory: IAppWindowViewFactory) =
    let appWindows = AppWindows.get ()

    window.Background <- SolidColorBrush(Color.FromScRgb(0.9f, 0.0f, 0.0f, 0.0f))
    window.Width <- (cellWidth * float appWindows.Length) + padding * 2.0
    window.Height <- cellHeight + padding * 2.0

    panel.Margin <- Thickness(padding)

    appWindows
    |> Seq.map (fun a -> viewFactory.Create(AppWindowViewModel(a)))
    |> Seq.iter (fun v -> panel.Children.Add(v) |> ignore)

    ()
