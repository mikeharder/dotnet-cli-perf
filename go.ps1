. ./common.ps1

$tmp = New-TemporaryFile 
Write-Host "Creating temp dir $tmp..."
Remove-Item $tmp -Force
New-Item $tmp -ItemType Directory | Out-Null
Set-Location $tmp
Write-Host

$env:GOPATH = $tmp

Measure-Command-With-Output { go get github.com/golang/example/hello }

Set-Location $tmp/src/github.com/golang/example/hello

Measure-Command-With-Output { go build }

Measure-Command-With-Output { go build }

Measure-Command-With-Output { Replace-Text ./hello.go "olleH" "2olleH" }

Measure-Command-With-Output { go build }

Measure-Command-With-Output { ./hello.exe }

Set-Location (Split-Path $MyInvocation.MyCommand.Path)
Remove-Item $tmp -Recurse -Force
