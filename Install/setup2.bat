@echo off
powershell -Command "python get-pip.py" & pip install virtualenv & pip install virtualenvwrapper-powershell
DEL /F /S /Q /A "get-pip.py"
set /p temp="Now run Setup3"