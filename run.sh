#!/bin/sh

cd ./build
dotnet app.dll "$@" || echo "run error code: $?"
