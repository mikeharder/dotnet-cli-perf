@echo off

docker build -t dotnet-cli-perf -f %~dp0/Dockerfile %~dp0/..
