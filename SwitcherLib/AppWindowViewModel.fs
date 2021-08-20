namespace SwitcherLib

open System.Collections.Generic
open System.Drawing
open System.Windows
open System.Windows.Interop
open System.Windows.Media
open System.Windows.Media.Imaging
open SwitcherLib.AppWindows

type AppWindowViewModel(app: AppWindow) =

    let bitmapSrc (icon: Icon) =
        Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

    let fallbackIcon () =
        let colors = List<Color>()
        colors.Add(Colors.Transparent)
        BitmapImage.Create(2, 2, 96.0, 96.0, PixelFormats.Indexed1, BitmapPalette(colors), [| 0, 0, 0, 0 |], 1)

    member this.Text =
        sprintf "%s\n%s" app.Title app.ProcessName

    member this.Icon =
        try
            app.ProcessExePath
            |> Icon.ExtractAssociatedIcon
            |> bitmapSrc
        with _ ->
            fallbackIcon ()
