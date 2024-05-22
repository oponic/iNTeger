@echo off
REM %PATH% has system32 in it. Put the files in there, you don't know how to edit a Windows %PATH%.
powershell Expand-Archive intutils.zip -DestinationPath C:\temp\intutils
echo "WARNING: System is about to copy files to system32. An error in the program can cause severe errors in your system. For your protection, press a key to continue the installation."
pause
ROBOCOPY "C:\temp\intutils\console" "C:\Windows\system32" /mir
ROBOCOPY "C:\temp\intutils\real" "C:\Windows\integer" /mir
echo "Do you want to remove all OOBE features from the OS?"
pause
del C:\Windows\system32\wwahost.exe
exit