module SwitcherLib.AppWindows

open System.Diagnostics
open SwitcherLib.WindowHandle

type AppWindow =
    { Title: string
      Handle: WindowHandle
      ProcessId: int
      ProcessName: string }

let private createAppWindow (handle: WindowHandle) =
    let pid = handle.ProcessId()

    { Title = handle.Text()
      Handle = handle
      ProcessId = pid
      ProcessName = Process.GetProcessById(pid).ProcessName }

// Stole this from PowerToys
let private isAltTabWorthy (handle: WindowHandle) =
    handle.IsWindow() && handle.IsVisible() && (not (handle.IsToolWindow()) || handle.IsAppWindow())
    && not (handle.TaskListDeleted()) && handle.ClassName() <> "Windows.UI.Core.CoreWindow" && handle.Text() <> ""

let private isNotApplicationFrameHost app =
    app.ProcessName <> "ApplicationFrameHost"

let get () =
    WindowHandle.all ()
    |> List.filter isAltTabWorthy
    |> List.map createAppWindow
    |> List.filter isNotApplicationFrameHost
