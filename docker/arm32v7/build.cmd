@echo off

docker build -t dotnet-cli-perf-arm32v7 -f %~dp0/Dockerfile %~dp0/../..
