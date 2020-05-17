module SwitcherLib.WindowIcon

open System.Diagnostics
open System.Drawing
open System.Windows
open System.Windows.Interop
open System.Windows.Media
open System.Windows.Media.Imaging

[<RequireQualifiedAccess>]
module WindowIcon =
    let private bitmapSrc (icon: Icon) =
        Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())

    let get (appProcess: Process) =
        try
            appProcess.Modules.[0].FileName
            |> Icon.ExtractAssociatedIcon
            |> bitmapSrc
        with ex -> failwith "Could not load application icon (TODO, need a fallback here)"
