module SwitcherLib.AppWindows

open SwitcherLib.WindowHandle

type AppWindows =
    { Title: string }

let private createAppWindow (handle: WindowHandle) =
    let title = WindowHandle.text handle

    match title with
    | "" -> None
    | title -> Some { Title = title }

let get () =
    WindowHandle.all ()
    |> List.filter WindowHandle.isVisible
    |> List.choose createAppWindow
