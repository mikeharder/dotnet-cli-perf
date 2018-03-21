#!/usr/bin/env bash

docker build -t dotnet-cli-perf-arm32v7 -f `dirname $0`/Dockerfile `dirname $0`/../../
