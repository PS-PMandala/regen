:: make.cmd
:: Run this file to build the program.

@ECHO OFF
SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

SET libs="netstandard.dll","Newtonsoft.Json.dll"
csc regen.cs /reference:%libs%