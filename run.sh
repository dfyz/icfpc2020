#!/bin/sh
cd ./build
dotnet app.dll "$@" || >&2 echo run error code: $?
