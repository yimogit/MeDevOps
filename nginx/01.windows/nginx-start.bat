@echo off
cd  /d %~dp0
echo kill nginx
taskkill /fi "imagename eq nginx.EXE" /f
echo start nginx
start nginx
echo start nginx success
pause