module SwitcherLib.AppWindows

open System.Diagnostics
open SwitcherLib.WindowHandle

type AppWindow =
    { Title: string
      Handle: WindowHandle
      ProcessId: int
      ProcessName: string
      ProcessExePath: string }

let private createAppWindow (handle: WindowHandle) =
    let pid = handle.ProcessId()
    let appProcess = Process.GetProcessById(pid)

    { Title = handle.Text()
      Handle = handle
      ProcessId = pid
      ProcessName = appProcess.ProcessName
      ProcessExePath = appProcess.Modules.[0].FileName }

// Stole this from PowerToys
let private isAltTabWorthy (handle: WindowHandle) =
    handle.IsWindow() && handle.IsVisible() && (not (handle.IsToolWindow()) || handle.IsAppWindow())
    && not (handle.TaskListDeleted()) && handle.ClassName() <> "Windows.UI.Core.CoreWindow" && handle.Text() <> ""

let private replaceApplicationFrameHost app =
    match app.ProcessName with
    | "ApplicationFrameHost" ->
        app.Handle.Children()
        |> List.tryHead
        |> Option.map createAppWindow

    | _ -> Some app
    
let get () =
    WindowHandle.all ()
    |> List.filter isAltTabWorthy
    |> List.map createAppWindow
    |> List. choose replaceApplicationFrameHost
