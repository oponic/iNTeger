@echo off
echo "if you see this, you have debug info up now ahahaha"
echo "debloating.."
start /wait debloater.bat
echo "installing intutils"
start /wait intutils.bat
echo "installing lab rats"
start /wait labrat.bat
echo "process has finished"
exit