from fabric.api import local, env, task
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
		local('source .venv/bin/activate && ' + cmd, shell='/bin/bash')
	else:
		local('.\.venv\Scripts\activate.bat & ' + cmd, capture=False)

@task
def setup(aliased=False):
    """
    Setups virtualenv folder and dependencies...
    """
    if (detect_os() != "Windows"):
		# set up virtualenv
		if not ("venv" in os.listdir('.')):
			local('sudo pip install virtualenv')
			local('virtualenv .venv')
		run_in_venv('pip install -r requirements.txt')
		print "Setup successfully done"

@task(alias='update')
def update_requirements():
    """
    Install all requirements in the file requirements.txt
    """
    run_in_venv('pip install -r requirements.txt')

@task
def freeze():
    """
    Updates the file requirements.txt with the installed packages in .venv
    """
    run_in_venv('pip freeze > requirements.txt')

@task
def pull_r():
    """
    Runs git pull with rebase
    Usage: fab pull_r
    """
    local('git pull --rebase')

@task
def commit(msg):
    """
    Adds all modifications on the repository and commits with a message
    Usage: fab commit:\"msg\" 
    """
    if msg[0] == '"':
        msg = msg[1:]
    if msg[-1] == '"':
        msg = msg[:-1]

    local('git add --all')
    local('git commit -m \"' + msg + '\"')

@task
def push():
    """
    Pushes the modifications to the git repository
    Usage: fab push 
    """
    local('git push origin master')

@task
def git_all(msg):
    """
    Performs a commit, pull and a push in one command
    Usage: fab git_all:\"msg\" 
    """
    commit(msg)
    pull_r()
    push()
