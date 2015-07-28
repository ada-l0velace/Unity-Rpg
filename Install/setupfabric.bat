@ECHO OFF
SETLOCAL
SET var=%path%
SET var2=%var:~-1%
powershell -Command "Start-BitsTransfer https://www.python.org/ftp/python/2.7.10/python-2.7.10.amd64.msi python-2.7.10.amd64.msi"
python-2.7.10.amd64.msi
setx path "%path%;C:\Python27;C:\Python27\Scripts"
powershell -Command "Start-BitsTransfer https://raw.github.com/pypa/pip/master/contrib/get-pip.py get-pip.py"
DEL /F /S /Q /A "python-2.7.10.amd64.msi"
set /p temp="Everything is now installed now run setup2.bat"
