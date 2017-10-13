. ./common.ps1

Write-Host "Clearing NuGet Cache..."
dotnet nuget locals all --clear
Write-Host

$tmp = New-TemporaryFile 
Write-Host "Creating temp dir $tmp..."
Remove-Item $tmp -Force
New-Item $tmp -ItemType Directory | Out-Null
Set-Location $tmp
Write-Host

New-Item console -ItemType Directory | Out-Null
Set-Location console

Measure-Command-With-Output { dotnet new console --no-restore }

# Terminate app after web server is started
(Get-Content ./Program.cs).Replace('Run()', 'RunAsync()') | Set-Content ./Program.cs

Measure-Command-With-Output { dotnet restore }

Measure-Command-With-Output { dotnet restore }

Measure-Command-With-Output { dotnet add package NUnit --version 3.8.1 --no-restore }

Measure-Command-With-Output { dotnet restore }

Measure-Command-With-Output { dotnet build }

Measure-Command-With-Output { dotnet build }

Measure-Command-With-Output { Add-Newline ./Program.cs }

Measure-Command-With-Output { dotnet build }

Measure-Command-With-Output { dotnet run }

Measure-Command-With-Output { Add-Newline ./Program.cs }

Measure-Command-With-Output { dotnet run }

Set-Location (Split-Path $MyInvocation.MyCommand.Path)
Remove-Item $tmp -Recurse -Force
