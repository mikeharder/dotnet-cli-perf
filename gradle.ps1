. ./common.ps1

Write-Host "Ensuring gradle daemon is running..."
gradle --daemon
Write-Host

$tmp = New-TemporaryFile 
Write-Host "Creating temp dir $tmp..."
Remove-Item $tmp -Force
New-Item $tmp -ItemType Directory | Out-Null
Set-Location $tmp
Write-Host

New-Item java-application -ItemType Directory | Out-Null
Set-Location java-application

Measure-Command-With-Output { gradle init --type java-application }

Measure-Command-With-Output { ./gradlew.bat assemble }

Measure-Command-With-Output { ./gradlew.bat assemble }

Measure-Command-With-Output { Replace-Text ./src/main/java/App.java "Hello" "Hello2" }

Measure-Command-With-Output { ./gradlew.bat assemble }

Measure-Command-With-Output { ./gradlew.bat run }

Measure-Command-With-Output { Replace-Text ./src/main/java/App.java "Hello" "Hello2" }

Measure-Command-With-Output { ./gradlew.bat run }

Set-Location (Split-Path $MyInvocation.MyCommand.Path)
Remove-Item $tmp -Recurse -Force
