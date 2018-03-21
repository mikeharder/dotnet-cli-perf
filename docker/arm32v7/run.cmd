@echo off

docker run -it --rm -v %~dp0/../../scenarios:/app/scenarios dotnet-cli-perf-arm32v7 %*
