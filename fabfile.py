from fabric.api import local, env
from fabric.operations import put, run
from fabric.utils import abort
import platform
import os
#this shit doesn't work on windows for obvious reasons need to check this later

# Do use this if you want to perform an action in virtualenv.
def detect_os():
	return platform.system()

def run_in_venv(cmd):
	if (detect_os() != "Windows"):
		local('source venv/bin/activate && ' + cmd, shell='/bin/bash')
	else:
		local('.\.venv\Scripts\activate.bat & ' + cmd, capture=False)

def setup(aliased=False):
    if (detect_os() != "Windows"):
		# set up virtualenv
		if not ("venv" in os.listdir('.')):
			local('sudo pip install virtualenv')
			local('virtualenv venv')
		run_in_venv('pip install -r requirements.txt')
		print "Setup successfully done"

def update_requirements():
    run_in_venv('pip install -r requirements.txt')

def freeze():
    run_in_venv('pip freeze > requirements.txt')

def pull_r():
    local('git pull --rebase')


def commit(msg):
    # fab commit:"msg"
    if msg[0] == '"':
        msg = msg[1:]
    if msg[-1] == '"':
        msg = msg[:-1]

    local('git add --all')
    local('git commit -m \"' + msg + '\"')


def push():
    local('git push origin master')


def git_all(msg):
    commit(msg)
    pull_r()
    push()