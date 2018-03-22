#!/usr/bin/env bash

docker run -it --rm -v $PWD/../../scenarios:/app/scenarios dotnet-cli-perf-arm32v7 "$@"
