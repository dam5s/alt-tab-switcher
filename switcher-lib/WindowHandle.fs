module SwitcherLib.WindowHandle

open System
open System.Text
open SwitcherLib.WindowNativeInterop

type WindowHandle(ptr: IntPtr) =
    let textLength () =
        Native.GetWindowTextLength(ptr)

    let readText (length: int) =
        let builder = StringBuilder(length)
        Native.GetWindowText(ptr, builder, builder.Capacity + 1) |> ignore
        builder.ToString()

    member this.Text() =
        let length = textLength ()
        if (length = 0) then "" else readText length

    member this.ProcessId() =
        let mutable id = uint32 0
        Native.GetWindowThreadProcessId(ptr, &id) |> ignore
        int id

    member this.ClassName() =
        let builder = StringBuilder(300)
        Native.GetClassName(ptr, builder, builder.MaxCapacity) |> ignore
        builder.ToString()

    member this.IsWindow() = Native.IsWindow ptr

    member this.IsVisible() = Native.IsWindowVisible ptr

    member this.IsAppWindow() =
        Native.GetWindowLong(ptr, Native.GWL_EXSTYLE) &&& Native.WS_EX_APPWINDOW = Native.WS_EX_APPWINDOW

    member this.IsToolWindow() =
        Native.GetWindowLong(ptr, Native.GWL_EXSTYLE) &&& Native.WS_EX_TOOLWINDOW = Native.WS_EX_TOOLWINDOW

    member this.TaskListDeleted() = Native.GetProp(ptr, "ITaskList_Deleted") <> IntPtr.Zero

    member this.SwitchTo() = Native.SwitchToThisWindow(ptr, true) |> ignore

[<RequireQualifiedAccess>]
module WindowHandle =
    let all (): WindowHandle list =
        let mutable windows: WindowHandle list = []

        let proc =
            fun ptr _ ->
                windows <- WindowHandle ptr :: windows
                Native.EnumWindowsContinueEnumerating

        Native.EnumWindows(Native.EnumWindowsProc(proc), IntPtr.Zero) |> ignore

        windows
