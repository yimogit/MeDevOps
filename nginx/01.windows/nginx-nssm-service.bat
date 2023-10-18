@echo off
cd  /d %~dp0
nssm stop Nginx-service
nssm remove Nginx-service confirm
nssm install Nginx-service D:\Software\nginx-1.24.0\start.bat
sc start Nginx-service
pause