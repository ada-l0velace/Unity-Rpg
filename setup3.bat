@echo off
virtualenv.exe -p C:\Python27\python.exe .venv
start cmd /k ".\.venv\Scripts\activate.bat & pip install -r requirements.txt"