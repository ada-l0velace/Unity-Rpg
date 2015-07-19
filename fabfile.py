from fabric.api import local, env
from fabric.operations import put, run
from fabric.utils import abort
import os
#this shit doesn't work on windows for obvious reasons need to check this later

# Do use this if you want to perform an action in virtualenv.
def run_in_venv(cmd):
    local('source venv/bin/activate && ' + cmd, shell='/bin/bash')

def setup(aliased=False):
    # set up virtualenv
    if not ("venv" in os.listdir('.')):
        local('sudo pip install virtualenv')
        local('virtualenv venv')
    run_in_venv('pip install -r requirements.txt')
    print "Setup successfully done"

def update_requirements(arg='s'):
    run_in_venv('pip install -r requirements.txt')
    install_sdk(arg)

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