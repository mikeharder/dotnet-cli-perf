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