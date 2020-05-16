module SwitcherLib.WindowHandle

open System
open System.Runtime.InteropServices
open System.Text

[<RequireQualifiedAccess>]
module private Native =
    type EnumWindowsProc = delegate of IntPtr * IntPtr -> bool

    let EnumWindowsContinueEnumerating = true

    [<DllImport("user32.dll")>]
    extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam)

    [<DllImport("user32.dll", CharSet = CharSet.Unicode)>]
    extern int GetWindowTextLength(IntPtr hWnd)

    [<DllImport("user32.dll", CharSet = CharSet.Unicode)>]
    extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount)

    [<DllImport("user32.dll", SetLastError = true)>]
    extern uint32 GetWindowThreadProcessId(IntPtr hWnd, [<Out>] uint32& processId)

    [<DllImport("user32.dll")>]
    extern bool IsWindowVisible(IntPtr hWnd)

    [<DllImport("user32.dll")>]
    extern void SwitchToThisWindow(IntPtr hWnd, bool usingAltTab)

type WindowHandle = private | WindowHandle of IntPtr

[<RequireQualifiedAccess>]
module WindowHandle =

    let private textLength (WindowHandle ptr) =
        Native.GetWindowTextLength(ptr)

    let private readText (length: int) (WindowHandle ptr) =
        let builder = StringBuilder(length)
        Native.GetWindowText(ptr, builder, builder.Capacity + 1) |> ignore
        builder.ToString()

    let text (handle: WindowHandle) =
        let length = textLength handle
        if (length = 0) then "" else readText length handle

    let processId (WindowHandle ptr): int =
        let mutable id = uint32 0
        Native.GetWindowThreadProcessId(ptr, &id) |> ignore
        int id

    let isVisible (WindowHandle ptr) =
        Native.IsWindowVisible ptr

    let switchTo (WindowHandle ptr) =
        Native.SwitchToThisWindow(ptr, true) |> ignore

    let all (): WindowHandle list =
        let mutable windows: WindowHandle list = []

        let proc =
            fun ptr _ ->
                windows <- WindowHandle ptr :: windows
                Native.EnumWindowsContinueEnumerating

        Native.EnumWindows(Native.EnumWindowsProc(proc), IntPtr.Zero) |> ignore

        windows
