@echo off
setlocal

cd /d "%~dp0.."
dotnet tool restore || exit /b 1
dotnet docfx Documentation/docfx.json --serve
exit /b %errorlevel%
