module SwitcherLib.WindowNativeInterop

open System
open System.Runtime.InteropServices
open System.Text

// https://docs.microsoft.com/en-us/windows/win32/api/winuser/

[<RequireQualifiedAccess>]
module Native =
    let GWL_EXSTYLE = -20
    let WS_EX_TOOLWINDOW = 0x0080
    let WS_EX_APPWINDOW = 0x40000

    type EnumWindowsProc = delegate of IntPtr * IntPtr -> bool

    [<Struct>]
    type WindowPlacement =
        { length: uint32
          flags: uint32
          showCmd: uint32 }

    let EnumWindowsContinueEnumerating = true

    [<DllImport("user32.dll")>]
    extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam)

    [<DllImport("user32.dll")>]
    extern bool IsWindow(IntPtr hWnd)

    [<DllImport("user32.dll")>]
    extern bool IsWindowVisible(IntPtr hWnd)

    [<DllImport("user32.dll", SetLastError = true)>]
    extern int GetWindowLong(IntPtr hWnd, int nIndex)

    [<DllImport("user32.dll", SetLastError = true)>]
    extern IntPtr GetProp(IntPtr hWnd, string lpString)

    [<DllImport("user32.dll")>]
    extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount)

    [<DllImport("user32.dll", CharSet = CharSet.Unicode)>]
    extern int GetWindowTextLength(IntPtr hWnd)

    [<DllImport("user32.dll", CharSet = CharSet.Unicode)>]
    extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount)

    [<DllImport("user32.dll", SetLastError = true)>]
    extern uint32 GetWindowThreadProcessId(IntPtr hWnd, [<Out>] uint32& processId)

    [<DllImport("user32.dll")>]
    extern void SwitchToThisWindow(IntPtr hWnd, bool usingAltTab)
