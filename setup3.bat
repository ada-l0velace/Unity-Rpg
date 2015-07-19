@echo off
virtualenv.exe -p C:\Python27\python.exe .venv
start cmd /k "Dependencies\VCForPython27.msi & .\.venv\Scripts\activate.bat & pip install -r requirements.txt"