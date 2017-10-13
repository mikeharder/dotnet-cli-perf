function Measure-Command-With-Output {
    Param(
        [ScriptBlock]$scriptBlock
    )
    Write-Host ">>>$scriptBlock"
    $m = Measure-Command { Invoke-Command $scriptBlock | Out-Host }
    $s = [math]::Round($m.TotalSeconds, 2)
    Write-Host "$($s)s`n"
}

function Add-Newline {
    Param(
        [string]$file
    )
    Add-Content $file ""
}

function Replace-Text {
    Param(
        [string]$file,
        [string]$from,
        [string]$to
    )
    (Get-Content $file).Replace($from, $to) | Set-Content $file
}
