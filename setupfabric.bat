@ECHO OFF
SETLOCAL
SET var=%path%
SET var2=%var:~-1%
powershell -Command "Invoke-WebRequest https://www.python.org/ftp/python/2.7.10/python-2.7.10.amd64.msi -OutFile python-2.7.10.amd64.msi"
python-2.7.10.amd64.msi
setx path "%path%;C:\Python27;C:\Python27\Scripts"
powershell -Command "Invoke-WebRequest https://raw.github.com/pypa/pip/master/contrib/get-pip.py -OutFile get-pip.py"
DEL /F /S /Q /A "python-2.7.10.amd64.msi"
DEL /F /S /Q /A "get-pip.py"
set /p temp="Everything is now installed now run setup2.bat"

