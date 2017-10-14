. ./common.ps1

$tmp = New-TemporaryFile 
Write-Host "Creating temp dir $tmp..."
Remove-Item $tmp -Force
New-Item $tmp -ItemType Directory | Out-Null
Set-Location $tmp
Write-Host

Copy-Item "$(Split-Path $MyInvocation.MyCommand.Path)/node/app.js" $tmp

Measure-Command-With-Output { node app.js }

Set-Location (Split-Path $MyInvocation.MyCommand.Path)
Remove-Item $tmp -Recurse -Force
