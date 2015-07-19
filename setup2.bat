@echo off
powershell -Command "python get-pip.py" & pip install virtualenv & pip install virtualenvwrapper-powershell
set /p temp="Now run Setup3"