module SwitcherLib.AppWindows

open System.Diagnostics
open SwitcherLib.WindowHandle

type AppWindow =
    { Title: string
      Handle: WindowHandle
      ProcessId: int
      ProcessName: string }

let private createAppWindow title (handle: WindowHandle) =
    let procId = WindowHandle.processId handle
    let proc = Process.GetProcessById procId

    { Title = title
      Handle = handle
      ProcessId = procId
      ProcessName = proc.ProcessName }

let private createAppWindowIfTitleNotEmpty (handle: WindowHandle) =
    let title = WindowHandle.text handle

    match title with
    | "" -> None
    | title -> Some(createAppWindow title handle)

let get () =
    WindowHandle.all ()
    |> List.filter WindowHandle.isVisible
    |> List.choose createAppWindowIfTitleNotEmpty
