@echo off
virtualenv.exe -p C:\Python27\python.exe .venv
powershell -Command "Start-BitsTransfer http://download.microsoft.com/download/7/9/6/796EF2E4-801B-4FC4-AB28-B59FBF6D907B/VCForPython27.msi VCForPython27.msi"
start cmd /k "VCForPython27.msi & DEL /F /S /Q /A VCForPython27.msi & .\.venv\Scripts\activate.bat & pip install -r requirements.txt"