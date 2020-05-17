module SwitcherLib.AppWindows

open System.Diagnostics
open System.Windows.Media
open SwitcherLib.WindowHandle
open SwitcherLib.WindowIcon

type AppWindow =
    { Title: string
      Handle: WindowHandle
      ProcessId: int
      ProcessName: string
      Icon: ImageSource }

let private createAppWindow (handle: WindowHandle) =
    let pid = handle.ProcessId()
    let appProcess = Process.GetProcessById(pid)

    { Title = handle.Text()
      Handle = handle
      ProcessId = pid
      ProcessName = appProcess.ProcessName
      Icon = WindowIcon.get appProcess }

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
