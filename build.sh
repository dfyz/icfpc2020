#!/bin/sh
dotnet publish -c release -o ./build || echo build error code: $?
