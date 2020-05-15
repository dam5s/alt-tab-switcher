module SwitcherLib.AppProcesses

open System.Diagnostics

type AppProcess =
    { Title: string
      Id: int
      Name: string }
    
let private hasWindow (p: Process) =
    p.MainWindowTitle <> ""
    
let private createProcess (p: Process) =
    { Title = p.MainWindowTitle
      Id = p.Id
      Name = p.ProcessName }

let get () =
    Process.GetProcesses()
    |> Array.toSeq
    |> Seq.filter hasWindow
    |> Seq.map createProcess
    |> Seq.toList
